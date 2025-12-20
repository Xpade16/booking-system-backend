using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using BookingSystem.Application.DTOs.Auth;
using BookingSystem.Application.Services;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;
using BookingSystem.Infrastructure.Data;
using BookingSystem.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookingSystem.Tests.Unit.Services;

public class AuthServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .ConfigureWarnings(warnings => 
            warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

        _context = new ApplicationDbContext(options);
        
        // Seed test data
        _context.Countries.Add(new Country 
        { 
            Id = 1, 
            Code = "SG", 
            Name = "Singapore", 
            TimeZone = "Asia/Singapore" 
        });
        _context.SaveChanges();

        // Setup mocks
        _tokenServiceMock = new Mock<ITokenService>();
        _emailServiceMock = new Mock<IEmailService>();
        _loggerMock = new Mock<ILogger<AuthService>>();

        _tokenServiceMock.Setup(x => x.GenerateJwtToken(It.IsAny<User>()))
            .Returns("mock-jwt-token");
        _tokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("mock-refresh-token");
        _tokenServiceMock.Setup(x => x.GenerateEmailVerificationToken())
            .Returns("mock-verification-token");

        _authService = new AuthService(
            _context,
            _tokenServiceMock.Object,
            _emailServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Email = "test@example.com",
            Password = "Test123!@#",
            FirstName = "Test",
            LastName = "User",
            CountryCode = "SG"
        };

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("mock-jwt-token");
        result.User.Email.Should().Be("test@example.com");
        result.User.FirstName.Should().Be("Test");
        result.User.IsEmailVerified.Should().BeFalse();

        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        userInDb.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowValidationException()
    {
        // Arrange
        var existingUser = new User
        {
            Email = "existing@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            FirstName = "Existing",
            LastName = "User",
            CountryId = 1
        };
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var request = new RegisterRequestDto
        {
            Email = "existing@example.com",
            Password = "Test123!@#",
            FirstName = "Test",
            LastName = "User",
            CountryCode = "SG"
        };

        // Act & Assert
        var action = async () => await _authService.RegisterAsync(request);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Email already registered");
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!@#"),
            FirstName = "Test",
            LastName = "User",
            CountryId = 1,
            Role = "User"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "Test123!@#"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("mock-jwt-token");
        result.User.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldThrowValidationException()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword123!"),
            FirstName = "Test",
            LastName = "User",
            CountryId = 1
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "WrongPassword123!"
        };

        // Act & Assert
        var action = async () => await _authService.LoginAsync(request);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Invalid email or password");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}