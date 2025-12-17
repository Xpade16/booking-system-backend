using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookingSystem.Application.Data;
using BookingSystem.Application.DTOs.Schedule;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;

namespace BookingSystem.Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ScheduleService> _logger;

    public ScheduleService(IApplicationDbContext context, ILogger<ScheduleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ClassScheduleDto>> GetSchedulesAsync(
        string? countryCode = null, 
        DateTime? startDate = null, 
        DateTime? endDate = null)
    {
        var query = _context.ClassSchedules
            .Include(cs => cs.Country)
            .Where(cs => cs.IsActive)
            .AsQueryable();

        // Filter by country
        if (!string.IsNullOrWhiteSpace(countryCode))
        {
            query = query.Where(cs => cs.Country.Code.ToUpper() == countryCode.ToUpper());
        }

        // Filter by date range
        if (startDate.HasValue)
        {
            query = query.Where(cs => cs.StartTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(cs => cs.StartTime <= endDate.Value);
        }

        var schedules = await query
            .OrderBy(cs => cs.StartTime)
            .Select(cs => new ClassScheduleDto
            {
                Id = cs.Id,
                Title = cs.Title,
                Description = cs.Description,
                CountryCode = cs.Country.Code,
                CountryName = cs.Country.Name,
                StartTime = cs.StartTime,
                EndTime = cs.EndTime,
                Capacity = cs.Capacity,
                AvailableSlots = cs.AvailableSlots,
                RequiredCredits = cs.RequiredCredits,
                IsActive = cs.IsActive
            })
            .ToListAsync();

        return schedules;
    }

    public async Task<ClassScheduleDto?> GetScheduleByIdAsync(int id)
    {
        var schedule = await _context.ClassSchedules
            .Include(cs => cs.Country)
            .Where(cs => cs.Id == id)
            .Select(cs => new ClassScheduleDto
            {
                Id = cs.Id,
                Title = cs.Title,
                Description = cs.Description,
                CountryCode = cs.Country.Code,
                CountryName = cs.Country.Name,
                StartTime = cs.StartTime,
                EndTime = cs.EndTime,
                Capacity = cs.Capacity,
                AvailableSlots = cs.AvailableSlots,
                RequiredCredits = cs.RequiredCredits,
                IsActive = cs.IsActive
            })
            .FirstOrDefaultAsync();

        return schedule;
    }

    public async Task<ClassScheduleDto> CreateScheduleAsync(CreateScheduleDto request)
    {
        // Validate country
        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Code.ToUpper() == request.CountryCode.ToUpper());

        if (country == null)
        {
            throw new NotFoundException($"Country with code '{request.CountryCode}' not found");
        }

        // Validate times
        if (request.StartTime >= request.EndTime)
        {
            throw new ValidationException("Start time must be before end time");
        }

        if (request.StartTime < DateTime.UtcNow)
        {
            throw new ValidationException("Cannot create class schedule in the past");
        }

        if (request.Capacity <= 0)
        {
            throw new ValidationException("Capacity must be greater than 0");
        }

        // Create schedule
        var schedule = new ClassSchedule
        {
            Title = request.Title,
            Description = request.Description,
            CountryId = country.Id,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Capacity = request.Capacity,
            AvailableSlots = request.Capacity, // Initially all slots available
            RequiredCredits = request.RequiredCredits,
            IsActive = true
        };

        _context.ClassSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Class schedule created: {Title} at {StartTime}", schedule.Title, schedule.StartTime);

        return new ClassScheduleDto
        {
            Id = schedule.Id,
            Title = schedule.Title,
            Description = schedule.Description,
            CountryCode = country.Code,
            CountryName = country.Name,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            Capacity = schedule.Capacity,
            AvailableSlots = schedule.AvailableSlots,
            RequiredCredits = schedule.RequiredCredits,
            IsActive = schedule.IsActive
        };
    }

    public async Task<ClassScheduleDto> UpdateScheduleAsync(int id, UpdateScheduleDto request)
    {
        var schedule = await _context.ClassSchedules
            .Include(cs => cs.Country)
            .FirstOrDefaultAsync(cs => cs.Id == id);

        if (schedule == null)
        {
            throw new NotFoundException("Class schedule not found");
        }

        // Update fields if provided
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            schedule.Title = request.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            schedule.Description = request.Description;
        }

        if (request.StartTime.HasValue)
        {
            if (request.StartTime.Value < DateTime.UtcNow)
            {
                throw new ValidationException("Cannot set start time in the past");
            }
            schedule.StartTime = request.StartTime.Value;
        }

        if (request.EndTime.HasValue)
        {
            schedule.EndTime = request.EndTime.Value;
        }

        if (schedule.StartTime >= schedule.EndTime)
        {
            throw new ValidationException("Start time must be before end time");
        }

        if (request.Capacity.HasValue)
        {
            if (request.Capacity.Value <= 0)
            {
                throw new ValidationException("Capacity must be greater than 0");
            }
            
            // Adjust available slots proportionally
            var bookedSlots = schedule.Capacity - schedule.AvailableSlots;
            schedule.Capacity = request.Capacity.Value;
            schedule.AvailableSlots = Math.Max(0, schedule.Capacity - bookedSlots);
        }

        if (request.RequiredCredits.HasValue)
        {
            schedule.RequiredCredits = request.RequiredCredits.Value;
        }

        if (request.IsActive.HasValue)
        {
            schedule.IsActive = request.IsActive.Value;
        }

        schedule.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Class schedule updated: {Id}", schedule.Id);

        return new ClassScheduleDto
        {
            Id = schedule.Id,
            Title = schedule.Title,
            Description = schedule.Description,
            CountryCode = schedule.Country.Code,
            CountryName = schedule.Country.Name,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            Capacity = schedule.Capacity,
            AvailableSlots = schedule.AvailableSlots,
            RequiredCredits = schedule.RequiredCredits,
            IsActive = schedule.IsActive
        };
    }

    public async Task<bool> DeleteScheduleAsync(int id)
    {
        var schedule = await _context.ClassSchedules
            .FirstOrDefaultAsync(cs => cs.Id == id);

        if (schedule == null)
        {
            throw new NotFoundException("Class schedule not found");
        }

        // Soft delete - just mark as inactive
        schedule.IsActive = false;
        schedule.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Class schedule deleted (soft): {Id}", schedule.Id);

        return true;
    }
}