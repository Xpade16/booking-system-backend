using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookingSystem.Application.Data;
using BookingSystem.Application.DTOs.Auth;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;

namespace BookingSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;
    private readonly Dictionary<string, int> _verificationTokens = new();

    public AuthService(
        IApplicationDbContext context,
        ITokenService tokenService,
        IEmailService emailService,
        ILogger<AuthService> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        // Check if user already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (existingUser != null)
        {
            throw new ValidationException("Email already registered");
        }

        // Get country
        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Code.ToUpper() == request.CountryCode.ToUpper());

        if (country == null)
        {
            throw new NotFoundException($"Country with code '{request.CountryCode}' not found");
        }

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Generate verification token (valid for 24 hours)
        var verificationToken = _tokenService.GenerateEmailVerificationToken();
        var tokenExpiry = DateTime.UtcNow.AddHours(24);

        // Create user
        var user = new User
        {
            Email = request.Email.ToLower(),
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CountryId = country.Id,
            IsEmailVerified = false,
            EmailVerificationToken = verificationToken,
            EmailVerificationTokenExpiry = tokenExpiry,
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Send verification email (mock)
        await _emailService.SendVerificationEmailAsync(user.Email, verificationToken);

        // Generate JWT
        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _logger.LogInformation("User registered successfully: {Email}", user.Email);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Find user
        var user = await _context.Users
            .Include(u => u.Country)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (user == null)
        {
            throw new ValidationException("Invalid email or password");
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new ValidationException("Invalid email or password");
        }

        // Generate tokens
        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _logger.LogInformation("User logged in successfully: {Email}", user.Email);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified
            }
        };
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ValidationException("Verification token is required");
        }

        // Find user by token
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.EmailVerificationToken == token);

        if (user == null)
        {
            throw new ValidationException("Invalid verification token");
        }

        // Check if token expired
        if (user.EmailVerificationTokenExpiry.HasValue && 
            user.EmailVerificationTokenExpiry.Value < DateTime.UtcNow)
        {
            throw new ValidationException("Verification token has expired");
        }

        // Mark as verified
        user.IsEmailVerified = true;
        user.EmailVerificationToken = null; // Clear token
        user.EmailVerificationTokenExpiry = null;
        
        await _context.SaveChangesAsync();

        _logger.LogInformation("Email verified successfully for user: {Email}", user.Email);

        return true;
    }
}