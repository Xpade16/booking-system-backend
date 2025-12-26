using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using FluentAssertions;
using BookingSystem.Application.Services;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookingSystem.Tests.Unit.Services;

public class BookingServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IRedisService> _redisServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ILogger<BookingService>> _loggerMock;
    private readonly BookingService _bookingService;
    private readonly User _testUser;
    private readonly ClassSchedule _testClass;
    private readonly UserPackage _testUserPackage;

    public BookingServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .ConfigureWarnings(warnings => 
            warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

        _context = new ApplicationDbContext(options);
        _redisServiceMock = new Mock<IRedisService>();
        _configurationMock = new Mock<IConfiguration>();
        // Seed test data
        var country = new Country 
        { 
            Id = 1, 
            Code = "SG", 
            Name = "Singapore", 
            TimeZone = "Asia/Singapore",
            IsActive = true
        };
        _context.Countries.Add(country);

        _testUser = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = "hash",
            FirstName = "Test",
            LastName = "User",
            CountryId = 1,
            Role = "User"
        };
        _context.Users.Add(_testUser);

        var package = new Package
        {
            Id = 1,
            Name = "Test Pack",
            Credits = 10,
            Price = 100.00m,
            CountryId = 1,
            ValidityDays = 30,
            IsActive = true
        };
        _context.Packages.Add(package);

        _testUserPackage = new UserPackage
        {
            Id = 1,
            UserId = 1,
            PackageId = 1,
            RemainingCredits = 10,
            PurchasedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsExpired = false,
            TransactionId = "txn_test"
        };
        _context.UserPackages.Add(_testUserPackage);

        _testClass = new ClassSchedule
        {
            Id = 1,
            Title = "Test Class",
            Description = "Test Description",
            CountryId = 1,
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            Capacity = 10,
            AvailableSlots = 10,
            RequiredCredits = 1,
            IsActive = true
        };
        _context.ClassSchedules.Add(_testClass);

        _context.SaveChanges();

        _loggerMock = new Mock<ILogger<BookingService>>();
        _bookingService = new BookingService(
            _context, 
            _redisServiceMock.Object,
            _configurationMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task BookClassAsync_WithValidData_ShouldCreateBooking()
    {
        // Act
        var result = await _bookingService.BookClassAsync(1, 1);

        // Assert
        result.Should().NotBeNull();
        result.BookingId.Should().BeGreaterThan(0);
        result.ClassTitle.Should().Be("Test Class");
        result.CreditsUsed.Should().Be(1);
        result.RemainingCredits.Should().Be(9);

        var bookingInDb = await _context.Bookings.FirstOrDefaultAsync();
        bookingInDb.Should().NotBeNull();
        bookingInDb!.UserId.Should().Be(1);
        bookingInDb.Status.Should().Be("Confirmed");

        var updatedClass = await _context.ClassSchedules.FindAsync(1);
        updatedClass!.AvailableSlots.Should().Be(9);

        var updatedPackage = await _context.UserPackages.FindAsync(1);
        updatedPackage!.RemainingCredits.Should().Be(9);
    }

    [Fact]
    public async Task BookClassAsync_WithInvalidClass_ShouldThrowNotFoundException()
    {
        // Act & Assert
        var action = async () => await _bookingService.BookClassAsync(1, 999);
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Class schedule not found");
    }

    [Fact]
    public async Task BookClassAsync_WithInsufficientCredits_ShouldThrowException()
    {
        // Arrange
        _testUserPackage.RemainingCredits = 0;
        await _context.SaveChangesAsync();

        // Act & Assert
        var action = async () => await _bookingService.BookClassAsync(1, 1);
        await action.Should().ThrowAsync<InsufficientCreditsException>();
    }

    [Fact]
    public async Task BookClassAsync_WhenAlreadyBooked_ShouldThrowBookingException()
    {
        // Arrange
        await _bookingService.BookClassAsync(1, 1);

        // Act & Assert
        var action = async () => await _bookingService.BookClassAsync(1, 1);
        await action.Should().ThrowAsync<BookingException>()
            .WithMessage("You have already booked this class");
    }

    [Fact]
    public async Task BookClassAsync_WhenClassFull_ShouldThrowBookingException()
    {
        // Arrange
        _testClass.AvailableSlots = 0;
        await _context.SaveChangesAsync();

        // Act & Assert
        var action = async () => await _bookingService.BookClassAsync(1, 1);
        await action.Should().ThrowAsync<BookingException>()
            .WithMessage("This class is fully booked");
    }

    [Fact]
    public async Task BookClassAsync_WithOverlappingClass_ShouldThrowBookingException()
    {
        // Arrange - Book first class
        await _bookingService.BookClassAsync(1, 1);

        // Add overlapping class
        var overlappingClass = new ClassSchedule
        {
            Id = 2,
            Title = "Overlapping Class",
            CountryId = 1,
            StartTime = _testClass.StartTime.AddMinutes(30),
            EndTime = _testClass.EndTime.AddMinutes(30),
            Capacity = 10,
            AvailableSlots = 10,
            RequiredCredits = 1,
            IsActive = true
        };
        _context.ClassSchedules.Add(overlappingClass);
        await _context.SaveChangesAsync();

        // Act & Assert
        var action = async () => await _bookingService.BookClassAsync(1, 2);
        await action.Should().ThrowAsync<BookingException>()
            .WithMessage("You have an overlapping booking at this time");
    }

    [Fact]
    public async Task CancelBookingAsync_MoreThan4Hours_ShouldRefundCredits()
    {
        // Arrange
        var booking = await _bookingService.BookClassAsync(1, 1);

        // Act
        var result = await _bookingService.CancelBookingAsync(1, booking.BookingId);

        // Assert
        result.Success.Should().BeTrue();
        result.IsRefunded.Should().BeTrue();
        result.RefundedCredits.Should().Be(1);

        var cancelledBooking = await _context.Bookings.FindAsync(booking.BookingId);
        cancelledBooking!.IsCancelled.Should().BeTrue();
        cancelledBooking.IsRefunded.Should().BeTrue();

        var updatedPackage = await _context.UserPackages.FindAsync(1);
        updatedPackage!.RemainingCredits.Should().Be(10); // Back to original

        var updatedClass = await _context.ClassSchedules.FindAsync(1);
        updatedClass!.AvailableSlots.Should().Be(10); // Slot restored
    }

    [Fact]
    public async Task CancelBookingAsync_LessThan4Hours_ShouldNotRefund()
    {
        // Arrange
        _testClass.StartTime = DateTime.UtcNow.AddHours(2); // Less than 4 hours
        await _context.SaveChangesAsync();

        var booking = await _bookingService.BookClassAsync(1, 1);

        // Act
        var result = await _bookingService.CancelBookingAsync(1, booking.BookingId);

        // Assert
        result.Success.Should().BeTrue();
        result.IsRefunded.Should().BeFalse();
        result.RefundedCredits.Should().Be(0);

        var updatedPackage = await _context.UserPackages.FindAsync(1);
        updatedPackage!.RemainingCredits.Should().Be(9); // Credits not refunded
    }

    [Fact]
    public async Task GetUserBookingsAsync_ShouldReturnUserBookings()
    {
        // Arrange
        await _bookingService.BookClassAsync(1, 1);

        // Act
        var result = await _bookingService.GetUserBookingsAsync(1);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result[0].ClassTitle.Should().Be("Test Class");
        result[0].Status.Should().Be("Confirmed");
    }

    [Fact]
    public async Task CheckInAsync_WithinWindow_ShouldSucceed()
    {
        // Arrange
        _testClass.StartTime = DateTime.UtcNow.AddMinutes(10);
        _testClass.EndTime = DateTime.UtcNow.AddHours(1);
        await _context.SaveChangesAsync();

        var booking = await _bookingService.BookClassAsync(1, 1);

        // Act
        var result = await _bookingService.CheckInAsync(1, booking.BookingId);

        // Assert
        result.Should().BeTrue();

        var checkedInBooking = await _context.Bookings.FindAsync(booking.BookingId);
        checkedInBooking!.CheckedInAt.Should().NotBeNull();
        checkedInBooking.Status.Should().Be("CheckedIn");
    }

    [Fact]
    public async Task CheckInAsync_TooEarly_ShouldThrowException()
    {
        // Arrange
        var booking = await _bookingService.BookClassAsync(1, 1);

        // Act & Assert
        var action = async () => await _bookingService.CheckInAsync(1, booking.BookingId);
        await action.Should().ThrowAsync<BookingException>()
            .WithMessage("Check-in opens 15 minutes before class start*");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}