namespace BookingSystem.Application.DTOs.Schedule;

public class UpdateScheduleDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Capacity { get; set; }
    public int? RequiredCredits { get; set; }
    public bool? IsActive { get; set; }
}