using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
    string GenerateEmailVerificationToken();
}