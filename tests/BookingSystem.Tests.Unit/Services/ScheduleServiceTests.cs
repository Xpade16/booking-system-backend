using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using BookingSystem.Application.DTOs.Schedule;
using BookingSystem.Application.Services;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookingSystem.Tests.Unit.Services;

public class ScheduleServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ILogger<ScheduleService>> _loggerMock;
    private readonly ScheduleService _scheduleService;

    public ScheduleServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .ConfigureWarnings(warnings => 
            warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

        _context = new ApplicationDbContext(options);
        
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

        var schedule = new ClassSchedule
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
        _context.ClassSchedules.Add(schedule);

        _context.SaveChanges();

        _loggerMock = new Mock<ILogger<ScheduleService>>();
        _scheduleService = new ScheduleService(_context, _loggerMock.Object);
    }

    [Fact]
    public async Task GetSchedulesAsync_WithNoFilters_ShouldReturnAllActiveSchedules()
    {
        // Act
        var result = await _scheduleService.GetSchedulesAsync();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result[0].Title.Should().Be("Test Class");
    }

    [Fact]
    public async Task GetSchedulesAsync_WithCountryFilter_ShouldReturnFilteredSchedules()
    {
        // Act
        var result = await _scheduleService.GetSchedulesAsync(countryCode: "SG");

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result[0].CountryCode.Should().Be("SG");
    }

    [Fact]
    public async Task GetScheduleByIdAsync_WithValidId_ShouldReturnSchedule()
    {
        // Act
        var result = await _scheduleService.GetScheduleByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Class");
        result.Capacity.Should().Be(10);
    }

    [Fact]
    public async Task GetScheduleByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = await _scheduleService.GetScheduleByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateScheduleAsync_WithValidData_ShouldCreateSchedule()
    {
        // Arrange
        var request = new CreateScheduleDto
        {
            Title = "New Class",
            Description = "New Description",
            CountryCode = "SG",
            StartTime = DateTime.UtcNow.AddDays(2),
            EndTime = DateTime.UtcNow.AddDays(2).AddHours(1),
            Capacity = 15,
            RequiredCredits = 1
        };

        // Act
        var result = await _scheduleService.CreateScheduleAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Class");
        result.Capacity.Should().Be(15);
        result.AvailableSlots.Should().Be(15);

        var scheduleInDb = await _context.ClassSchedules.FindAsync(result.Id);
        scheduleInDb.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateScheduleAsync_WithInvalidCountry_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new CreateScheduleDto
        {
            Title = "New Class",
            Description = "New Description",
            CountryCode = "XX",
            StartTime = DateTime.UtcNow.AddDays(2),
            EndTime = DateTime.UtcNow.AddDays(2).AddHours(1),
            Capacity = 15,
            RequiredCredits = 1
        };

        // Act & Assert
        var action = async () => await _scheduleService.CreateScheduleAsync(request);
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Country with code 'XX' not found");
    }

    [Fact]
    public async Task CreateScheduleAsync_WithPastStartTime_ShouldThrowValidationException()
    {
        // Arrange
        var request = new CreateScheduleDto
        {
            Title = "New Class",
            Description = "New Description",
            CountryCode = "SG",
            StartTime = DateTime.UtcNow.AddDays(-1), // Past
            EndTime = DateTime.UtcNow.AddHours(1),
            Capacity = 15,
            RequiredCredits = 1
        };

        // Act & Assert
        var action = async () => await _scheduleService.CreateScheduleAsync(request);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Cannot create class schedule in the past");
    }

    [Fact]
    public async Task UpdateScheduleAsync_WithValidData_ShouldUpdateSchedule()
    {
        // Arrange
        var request = new UpdateScheduleDto
        {
            Title = "Updated Title",
            Capacity = 20
        };

        // Act
        var result = await _scheduleService.UpdateScheduleAsync(1, request);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Updated Title");
        result.Capacity.Should().Be(20);
    }

    [Fact]
    public async Task UpdateScheduleAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new UpdateScheduleDto
        {
            Title = "Updated Title"
        };

        // Act & Assert
        var action = async () => await _scheduleService.UpdateScheduleAsync(999, request);
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Class schedule not found");
    }

    [Fact]
    public async Task DeleteScheduleAsync_WithValidId_ShouldMarkAsInactive()
    {
        // Act
        var result = await _scheduleService.DeleteScheduleAsync(1);

        // Assert
        result.Should().BeTrue();

        var scheduleInDb = await _context.ClassSchedules.FindAsync(1);
        scheduleInDb.Should().NotBeNull();
        scheduleInDb!.IsActive.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}