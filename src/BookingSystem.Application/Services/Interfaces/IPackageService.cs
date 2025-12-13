using BookingSystem.Application.DTOs.Package;

namespace BookingSystem.Application.Services.Interfaces;

public interface IPackageService
{
    Task<List<PackageDto>> GetPackagesByCountryAsync(string countryCode);
    Task<PurchasePackageResponseDto> PurchasePackageAsync(int userId, PurchasePackageRequestDto request);
    Task<List<UserPackageDto>> GetUserPackagesAsync(int userId);
    Task MarkExpiredPackagesAsync();
}