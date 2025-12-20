namespace BookingSystem.Domain.Entities;

public class UserPackage
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PackageId { get; set; }
    public int RemainingCredits { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public bool IsExpired { get; set; } = false;
    public string TransactionId { get; set; } = string.Empty;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Package Package { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}