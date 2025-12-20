namespace BookingSystem.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ClassScheduleId { get; set; }
    public int UserPackageId { get; set; }
    public int CreditsUsed { get; set; }
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CheckedInAt { get; set; }
    public bool IsCancelled { get; set; } = false;
    public DateTime? CancelledAt { get; set; }
    public bool IsRefunded { get; set; } = false;
    public string Status { get; set; } = "Confirmed"; // Confirmed, CheckedIn, Cancelled
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    
    // Navigation properties
    public User User { get; set; } = null!;
    public ClassSchedule ClassSchedule { get; set; } = null!;
    public UserPackage UserPackage { get; set; } = null!;
}