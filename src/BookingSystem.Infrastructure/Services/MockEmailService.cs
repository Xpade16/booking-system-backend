using Microsoft.Extensions.Logging;
using BookingSystem.Application.Services.Interfaces;

namespace BookingSystem.Infrastructure.Services;

public class MockEmailService : IEmailService
{
    private readonly ILogger<MockEmailService> _logger;

    public MockEmailService(ILogger<MockEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendVerificationEmailAsync(string email, string token)
    {
        _logger.LogInformation(
            "📧 MOCK EMAIL: Verification email to {Email}\n" +
            "   Token: {Token}\n" +
            "   Verification URL: http://localhost:5000/api/auth/verify?token={Token}",
            email, token, token);
        
        // In production, integrate with SendGrid, AWS SES, Mailgun, etc.
        return Task.CompletedTask;
    }

    public Task SendPasswordResetEmailAsync(string email, string token)
    {
        _logger.LogInformation(
            "📧 MOCK EMAIL: Password reset email to {Email}\n" +
            "   Token: {Token}\n" +
            "   Reset URL: http://localhost:5000/api/auth/reset-password?token={Token}",
            email, token, token);
        
        return Task.CompletedTask;
    }
}