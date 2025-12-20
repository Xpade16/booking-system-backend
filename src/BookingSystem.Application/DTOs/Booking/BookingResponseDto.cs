namespace BookingSystem.Application.DTOs.Booking;

public class BookingResponseDto
{
    public int BookingId { get; set; }
    public string ClassTitle { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int CreditsUsed { get; set; }
    public int RemainingCredits { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}