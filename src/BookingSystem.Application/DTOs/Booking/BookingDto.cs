namespace BookingSystem.Application.DTOs.Booking;

public class BookingDto
{
    public int Id { get; set; }
    public int ClassScheduleId { get; set; }
    public string ClassTitle { get; set; } = string.Empty;
    public DateTime ClassStartTime { get; set; }
    public DateTime ClassEndTime { get; set; }
    public int CreditsUsed { get; set; }
    public DateTime BookedAt { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }
    public bool IsRefunded { get; set; }
    public bool CanCancel { get; set; }
    public bool CanCheckIn { get; set; }
}