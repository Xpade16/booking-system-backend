using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookingSystem.Application.Data;
using BookingSystem.Application.DTOs.Package;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;

namespace BookingSystem.Application.Services;

public class PackageService : IPackageService
{
    private readonly IApplicationDbContext _context;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PackageService> _logger;

    public PackageService(
        IApplicationDbContext context,
        IPaymentService paymentService,
        ILogger<PackageService> logger)
    {
        _context = context;
        _paymentService = paymentService;
        _logger = logger;
    }

    public async Task<List<PackageDto>> GetPackagesByCountryAsync(string countryCode)
    {
        var packages = await _context.Packages
            .Include(p => p.Country)
            .Where(p => p.Country.Code.ToUpper() == countryCode.ToUpper() && p.IsActive)
            .OrderBy(p => p.Price)
            .Select(p => new PackageDto
            {
                Id = p.Id,
                Name = p.Name,
                Credits = p.Credits,
                Price = p.Price,
                ValidityDays = p.ValidityDays,
                CountryCode = p.Country.Code,
                CountryName = p.Country.Name
            })
            .ToListAsync();

        return packages;
    }

    public async Task<PurchasePackageResponseDto> PurchasePackageAsync(int userId, PurchasePackageRequestDto request)
    {
        // Get package
        var package = await _context.Packages
            .Include(p => p.Country)
            .FirstOrDefaultAsync(p => p.Id == request.PackageId && p.IsActive);

        if (package == null)
        {
            throw new NotFoundException("Package not found or not available");
        }

        // Get user
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        // Verify country match
        if (user.CountryId != package.CountryId)
        {
            throw new ValidationException("Package not available in your country");
        }

        // Process payment
        _logger.LogInformation("Processing payment for user {UserId}, package {PackageId}", userId, package.Id);

        var paymentResult = await _paymentService.ChargeAsync(
            package.Price,
            "USD", // In production, use country currency
            request.PaymentToken);

        if (!paymentResult.Success)
        {
            throw new ValidationException($"Payment failed: {paymentResult.ErrorMessage}");
        }

        // Create user package
        var userPackage = new UserPackage
        {
            UserId = userId,
            PackageId = package.Id,
            RemainingCredits = package.Credits,
            PurchasedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(package.ValidityDays),
            IsExpired = false,
            TransactionId = paymentResult.TransactionId
        };

        _context.UserPackages.Add(userPackage);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Package purchased successfully: User {UserId}, Package {PackageId}, Transaction {TransactionId}",
            userId, package.Id, paymentResult.TransactionId);

        return new PurchasePackageResponseDto
        {
            UserPackageId = userPackage.Id,
            PackageName = package.Name,
            Credits = package.Credits,
            PurchasedAt = userPackage.PurchasedAt,
            ExpiresAt = userPackage.ExpiresAt,
            TransactionId = userPackage.TransactionId,
            AmountPaid = package.Price
        };
    }

    public async Task<List<UserPackageDto>> GetUserPackagesAsync(int userId)
    {
        var now = DateTime.UtcNow;

        var raw = await _context.UserPackages
            .Include(up => up.Package)
            .Where(up => up.UserId == userId)
            .OrderByDescending(up => up.PurchasedAt)
            .Select(up => new
            {
                up.Id,
                PackageName = up.Package.Name,
                OriginalCredits = up.Package.Credits,
                up.RemainingCredits,
                up.PurchasedAt,
                up.ExpiresAt,
                up.IsExpired
            })
            .AsNoTracking()
            .ToListAsync();

        var userPackages = raw.Select(up => new UserPackageDto
        {
            Id = up.Id,
            PackageName = up.PackageName,
            OriginalCredits = up.OriginalCredits,
            RemainingCredits = up.RemainingCredits,
            PurchasedAt = up.PurchasedAt,
            ExpiresAt = up.ExpiresAt,
            IsExpired = up.IsExpired,
            DaysRemaining = up.IsExpired
                ? 0
                : Math.Max(0, (int)(up.ExpiresAt - now).TotalDays)
        }).ToList();

        return userPackages;
    }

    public async Task MarkExpiredPackagesAsync()
    {
        var now = DateTime.UtcNow;

        var expiredPackages = await _context.UserPackages
            .Where(up => !up.IsExpired && up.ExpiresAt <= now)
            .ToListAsync();

        if (expiredPackages.Any())
        {
            foreach (var package in expiredPackages)
            {
                package.IsExpired = true;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Marked {Count} packages as expired", expiredPackages.Count);
        }
    }
}