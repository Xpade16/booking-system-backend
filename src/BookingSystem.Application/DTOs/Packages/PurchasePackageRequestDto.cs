namespace BookingSystem.Application.DTOs.Package;

public class PurchasePackageRequestDto
{
    public int PackageId { get; set; }
    public string PaymentToken { get; set; } = string.Empty;
}