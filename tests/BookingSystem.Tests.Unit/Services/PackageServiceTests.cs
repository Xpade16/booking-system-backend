using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using BookingSystem.Application.DTOs.Package;
using BookingSystem.Application.Services;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookingSystem.Tests.Unit.Services;

public class PackageServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IPaymentService> _paymentServiceMock;
    private readonly Mock<ILogger<PackageService>> _loggerMock;
    private readonly PackageService _packageService;

    public PackageServiceTests()
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

        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = "hash",
            FirstName = "Test",
            LastName = "User",
            CountryId = 1,
            Role = "User"
        };
        _context.Users.Add(user);

        _context.SaveChanges();

        _paymentServiceMock = new Mock<IPaymentService>();
        _loggerMock = new Mock<ILogger<PackageService>>();

        _packageService = new PackageService(
            _context,
            _paymentServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetPackagesByCountryAsync_WithValidCountryCode_ShouldReturnPackages()
    {
        // Act
        var result = await _packageService.GetPackagesByCountryAsync("SG");

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Test Pack");
        result[0].Credits.Should().Be(10);
        result[0].Price.Should().Be(100.00m);
    }

    [Fact]
    public async Task GetPackagesByCountryAsync_WithInvalidCountryCode_ShouldReturnEmptyList()
    {
        // Act
        var result = await _packageService.GetPackagesByCountryAsync("XX");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task PurchasePackageAsync_WithValidData_ShouldCreateUserPackage()
    {
        // Arrange
        _paymentServiceMock.Setup(x => x.ChargeAsync(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new PaymentResult
            {
                Success = true,
                TransactionId = "txn_test123",
                ProcessedAt = DateTime.UtcNow,
                Amount = 100.00m,
                Currency = "USD"
            });

        var request = new PurchasePackageRequestDto
        {
            PackageId = 1,
            PaymentToken = "test_token"
        };

        // Act
        var result = await _packageService.PurchasePackageAsync(1, request);

        // Assert
        result.Should().NotBeNull();
        result.PackageName.Should().Be("Test Pack");
        result.Credits.Should().Be(10);
        result.TransactionId.Should().Be("txn_test123");

        var userPackageInDb = await _context.UserPackages.FirstOrDefaultAsync();
        userPackageInDb.Should().NotBeNull();
        userPackageInDb!.RemainingCredits.Should().Be(10);
        userPackageInDb.IsExpired.Should().BeFalse();
    }

    [Fact]
    public async Task PurchasePackageAsync_WithPaymentFailure_ShouldThrowValidationException()
    {
        // Arrange
        _paymentServiceMock.Setup(x => x.ChargeAsync(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new PaymentResult
            {
                Success = false,
                ErrorMessage = "Card declined"
            });

        var request = new PurchasePackageRequestDto
        {
            PackageId = 1,
            PaymentToken = "test_token"
        };

        // Act & Assert
        var action = async () => await _packageService.PurchasePackageAsync(1, request);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Payment failed: Card declined");
    }

    [Fact]
    public async Task PurchasePackageAsync_WithInvalidPackage_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new PurchasePackageRequestDto
        {
            PackageId = 999,
            PaymentToken = "test_token"
        };

        // Act & Assert
        var action = async () => await _packageService.PurchasePackageAsync(1, request);
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Package not found or not available");
    }

    [Fact]
    public async Task GetUserPackagesAsync_ShouldReturnUserPackages()
    {
        // Arrange
        var userPackage = new UserPackage
        {
            UserId = 1,
            PackageId = 1,
            RemainingCredits = 8,
            PurchasedAt = DateTime.UtcNow.AddDays(-5),
            ExpiresAt = DateTime.UtcNow.AddDays(25),
            IsExpired = false,
            TransactionId = "txn_123"
        };
        _context.UserPackages.Add(userPackage);
        await _context.SaveChangesAsync();

        // Act
        var result = await _packageService.GetUserPackagesAsync(1);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result[0].PackageName.Should().Be("Test Pack");
        result[0].RemainingCredits.Should().Be(8);
        result[0].IsExpired.Should().BeFalse();
    }

    [Fact]
    public async Task MarkExpiredPackagesAsync_ShouldMarkExpiredPackages()
    {
        // Arrange
        var expiredPackage = new UserPackage
        {
            UserId = 1,
            PackageId = 1,
            RemainingCredits = 5,
            PurchasedAt = DateTime.UtcNow.AddDays(-40),
            ExpiresAt = DateTime.UtcNow.AddDays(-10), // Expired 10 days ago
            IsExpired = false,
            TransactionId = "txn_123"
        };
        _context.UserPackages.Add(expiredPackage);
        await _context.SaveChangesAsync();

        // Act
        await _packageService.MarkExpiredPackagesAsync();

        // Assert
        var packageInDb = await _context.UserPackages.FirstOrDefaultAsync();
        packageInDb.Should().NotBeNull();
        packageInDb!.IsExpired.Should().BeTrue();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}