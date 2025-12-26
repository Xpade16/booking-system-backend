using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using BookingSystem.Application.Data;
using BookingSystem.Application.DTOs.Booking;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;

namespace BookingSystem.Application.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly IRedisService _redisService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BookingService> _logger;
    private readonly bool _useRedisForConcurrency;

    public BookingService(
        IApplicationDbContext context,
        IRedisService redisService,
        IConfiguration configuration,
        ILogger<BookingService> logger)
    {
        _context = context;
        _redisService = redisService;
        _configuration = configuration;
        _logger = logger;
        _useRedisForConcurrency = configuration.GetValue<bool>("Features:UseRedisForConcurrency", true);
    }

    public async Task<BookingResponseDto> BookClassAsync(int userId, int classScheduleId)
    {
        // 1. Get and validate class schedule
        var classSchedule = await _context.ClassSchedules
            .FirstOrDefaultAsync(cs => cs.Id == classScheduleId);

        if (classSchedule == null)
        {
            throw new NotFoundException("Class schedule not found");
        }

        if (!classSchedule.IsActive)
        {
            throw new BookingException("This class is no longer available");
        }

        if (classSchedule.StartTime <= DateTime.UtcNow)
        {
            throw new BookingException("Cannot book a class that has already started");
        }

        // 2. Check for existing booking
        var existingBooking = await _context.Bookings
            .FirstOrDefaultAsync(b =>
                b.UserId == userId &&
                b.ClassScheduleId == classScheduleId &&
                !b.IsCancelled);

        if (existingBooking != null)
        {
            throw new BookingException("You have already booked this class");
        }

        // 3. Check for overlapping bookings
        var hasOverlap = await _context.Bookings
            .Include(b => b.ClassSchedule)
            .AnyAsync(b =>
                b.UserId == userId &&
                !b.IsCancelled &&
                ((b.ClassSchedule.StartTime < classSchedule.EndTime &&
                  b.ClassSchedule.EndTime > classSchedule.StartTime)));

        if (hasOverlap)
        {
            throw new BookingException("You have an overlapping booking at this time");
        }

        // 4. Get active user package with sufficient credits
        var userPackage = await _context.UserPackages
            .Include(up => up.Package)
            .Where(up =>
                up.UserId == userId &&
                !up.IsExpired &&
                up.ExpiresAt > DateTime.UtcNow &&
                up.RemainingCredits >= classSchedule.RequiredCredits)
            .OrderBy(up => up.ExpiresAt)
            .FirstOrDefaultAsync();

        if (userPackage == null)
        {
            throw new InsufficientCreditsException(
                $"Insufficient credits. This class requires {classSchedule.RequiredCredits} credit(s). " +
                "Please purchase a package to continue.");
        }

        // 5. Try to reserve slot using Redis or DB
        bool slotReserved = false;
        bool useRedis = _useRedisForConcurrency; // Local variable for this request

        if (useRedis)
        {
            try
            {
                // Ensure Redis has the current slot count
                var redisSlots = await _redisService.GetAvailableSlotsAsync(classScheduleId);
                if (redisSlots == -1)
                {
                    // Key doesn't exist, initialize it
                    await _redisService.SetAvailableSlotsAsync(classScheduleId, classSchedule.AvailableSlots);
                }

                // Try atomic decrement in Redis
                slotReserved = await _redisService.TryDecrementSlotAsync(classScheduleId);

                if (slotReserved)
                {
                    _logger.LogInformation("🔴 Redis: Slot reserved for class {ClassId}", classScheduleId);
                }
                else
                {
                    _logger.LogWarning("🔴 Redis: No slots available for class {ClassId}", classScheduleId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "🔴 Redis: Error, falling back to database");
                useRedis = false;
            }
        }

        if (!useRedis)
        {
            // Fallback to DB transaction
            _logger.LogInformation("💾 DB: Using database for slot reservation");

            if (classSchedule.AvailableSlots <= 0)
            {
                throw new BookingException("This class is fully booked");
            }

            slotReserved = true;
            classSchedule.AvailableSlots -= 1;
        }

        if (!slotReserved)
        {
            throw new BookingException("This class is fully booked");
        }

        // 6. Create booking and update credits atomically
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Deduct credits
            userPackage.RemainingCredits -= classSchedule.RequiredCredits;

            // Update DB slots based on mode
            if (!useRedis)
            {
                // Already decremented above
            }
            else
            {
                // Sync DB with Redis
                classSchedule.AvailableSlots = await _redisService.GetAvailableSlotsAsync(classScheduleId);
            }

            // Create booking
            var booking = new Booking
            {
                UserId = userId,
                ClassScheduleId = classScheduleId,
                UserPackageId = userPackage.Id,
                CreditsUsed = classSchedule.RequiredCredits,
                BookedAt = DateTime.UtcNow,
                Status = "Confirmed"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            _logger.LogInformation(
                "✅ Booking created: User {UserId}, Class {ClassId}, Credits {Credits}",
                userId, classScheduleId, classSchedule.RequiredCredits);

            return new BookingResponseDto
            {
                BookingId = booking.Id,
                ClassTitle = classSchedule.Title,
                StartTime = classSchedule.StartTime,
                EndTime = classSchedule.EndTime,
                CreditsUsed = booking.CreditsUsed,
                RemainingCredits = userPackage.RemainingCredits,
                Status = booking.Status,
                Message = "Class booked successfully!"
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            // Rollback Redis slot if it was decremented
            if (useRedis && slotReserved)
            {
                await _redisService.IncrementSlotAsync(classScheduleId);
                _logger.LogWarning("🔴 Redis: Slot rollback for class {ClassId}", classScheduleId);
            }

            _logger.LogError(ex, "Failed to create booking for user {UserId}", userId);
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }

    public async Task<CancelBookingResponseDto> CancelBookingAsync(int userId, int bookingId)
    {
        // 1. Get booking with related data
        var booking = await _context.Bookings
            .Include(b => b.ClassSchedule)
            .Include(b => b.UserPackage)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null)
        {
            throw new NotFoundException("Booking not found");
        }

        if (booking.UserId != userId)
        {
            throw new BookingException("You can only cancel your own bookings");
        }

        if (booking.IsCancelled)
        {
            throw new BookingException("This booking has already been cancelled");
        }

        if (booking.ClassSchedule.StartTime <= DateTime.UtcNow)
        {
            throw new BookingException("Cannot cancel a class that has already started");
        }

        // 2. Check refund eligibility (>= 4 hours before class)
        var hoursUntilClass = (booking.ClassSchedule.StartTime - DateTime.UtcNow).TotalHours;
        var isRefundable = hoursUntilClass >= 4;

        // 3. Cancel booking and process refund
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Mark as cancelled
            booking.IsCancelled = true;
            booking.CancelledAt = DateTime.UtcNow;
            booking.Status = "Cancelled";

            // Refund credits if eligible
            if (isRefundable)
            {
                booking.UserPackage.RemainingCredits += booking.CreditsUsed;
                booking.IsRefunded = true;
            }

            // Increment available slots in DB
            booking.ClassSchedule.AvailableSlots += 1;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Increment slot in Redis
            if (_useRedisForConcurrency)
            {
                try
                {
                    await _redisService.IncrementSlotAsync(booking.ClassScheduleId);
                    _logger.LogInformation("🔴 Redis: Slot incremented after cancellation for class {ClassId}",
                        booking.ClassScheduleId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "🔴 Redis: Failed to increment slot after cancellation");
                    // Don't fail the cancellation if Redis update fails
                }
            }

            _logger.LogInformation(
                "✅ Booking cancelled: Id {BookingId}, Refunded: {IsRefunded}",
                bookingId, isRefundable);

            var message = isRefundable
                ? $"Booking cancelled successfully. {booking.CreditsUsed} credit(s) have been refunded."
                : "Booking cancelled. No refund as cancellation was made less than 4 hours before class start.";

            return new CancelBookingResponseDto
            {
                Success = true,
                IsRefunded = isRefundable,
                RefundedCredits = isRefundable ? booking.CreditsUsed : 0,
                Message = message
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to cancel booking {BookingId}", bookingId);
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }

    public async Task<List<BookingDto>> GetUserBookingsAsync(int userId, string? status = null)
    {
        var query = _context.Bookings
            .Include(b => b.ClassSchedule)
            .Where(b => b.UserId == userId);

        // Filter by status if provided
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(b => b.Status.ToLower() == status.ToLower());
        }

        var bookings = await query
            .OrderByDescending(b => b.BookedAt)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                ClassScheduleId = b.ClassScheduleId,
                ClassTitle = b.ClassSchedule.Title,
                ClassStartTime = b.ClassSchedule.StartTime,
                ClassEndTime = b.ClassSchedule.EndTime,
                CreditsUsed = b.CreditsUsed,
                BookedAt = b.BookedAt,
                CheckedInAt = b.CheckedInAt,
                Status = b.Status,
                IsCancelled = b.IsCancelled,
                IsRefunded = b.IsRefunded,
                CanCancel = !b.IsCancelled && b.ClassSchedule.StartTime > DateTime.UtcNow,
                CanCheckIn = !b.IsCancelled &&
                            b.CheckedInAt == null &&
                            b.ClassSchedule.StartTime <= DateTime.UtcNow.AddMinutes(15) &&
                            b.ClassSchedule.EndTime >= DateTime.UtcNow
            })
            .ToListAsync();

        return bookings;
    }

    public async Task<bool> CheckInAsync(int userId, int bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.ClassSchedule)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null)
        {
            throw new NotFoundException("Booking not found");
        }

        if (booking.UserId != userId)
        {
            throw new BookingException("You can only check in to your own bookings");
        }

        if (booking.IsCancelled)
        {
            throw new BookingException("Cannot check in to a cancelled booking");
        }

        if (booking.CheckedInAt != null)
        {
            throw new BookingException("You have already checked in to this class");
        }

        // Check if within check-in window (15 minutes before to class end)
        var now = DateTime.UtcNow;
        var checkInWindowStart = booking.ClassSchedule.StartTime.AddMinutes(-15);
        var checkInWindowEnd = booking.ClassSchedule.EndTime;

        if (now < checkInWindowStart)
        {
            var minutesUntil = (checkInWindowStart - now).TotalMinutes;
            throw new BookingException(
                $"Check-in opens 15 minutes before class start. Please try again in {Math.Ceiling(minutesUntil)} minute(s).");
        }

        if (now > checkInWindowEnd)
        {
            throw new BookingException("Check-in window has closed. This class has ended.");
        }

        // Check in
        booking.CheckedInAt = DateTime.UtcNow;
        booking.Status = "CheckedIn";

        await _context.SaveChangesAsync();

        _logger.LogInformation("User {UserId} checked in to booking {BookingId}", userId, bookingId);

        return true;
    }
}