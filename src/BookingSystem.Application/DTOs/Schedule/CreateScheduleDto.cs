namespace BookingSystem.Application.DTOs.Schedule;

public class CreateScheduleDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
    public int RequiredCredits { get; set; } = 1;
}