namespace BookingSystem.Application.DTOs.Package;

public class PurchasePackageResponseDto
{
    public int UserPackageId { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int Credits { get; set; }
    public DateTime PurchasedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
}