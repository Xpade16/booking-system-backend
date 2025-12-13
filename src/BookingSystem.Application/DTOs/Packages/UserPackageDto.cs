namespace BookingSystem.Application.DTOs.Package;

public class UserPackageDto
{
    public int Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int OriginalCredits { get; set; }
    public int RemainingCredits { get; set; }
    public DateTime PurchasedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsExpired { get; set; }
    public int DaysRemaining { get; set; }
}