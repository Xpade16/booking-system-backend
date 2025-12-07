using BookingSystem.Application.DTOs.Auth;

namespace BookingSystem.Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<bool> VerifyEmailAsync(string token);
}