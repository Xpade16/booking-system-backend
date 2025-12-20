namespace BookingSystem.Application.DTOs.Booking;

public class CancelBookingResponseDto
{
    public bool Success { get; set; }
    public bool IsRefunded { get; set; }
    public int RefundedCredits { get; set; }
    public string Message { get; set; } = string.Empty;
}