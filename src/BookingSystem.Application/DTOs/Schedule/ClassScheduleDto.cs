namespace BookingSystem.Application.DTOs.Schedule;

public class ClassScheduleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
    public int AvailableSlots { get; set; }
    public int RequiredCredits { get; set; }
    public bool IsActive { get; set; }
    public bool IsFull => AvailableSlots <= 0;
}