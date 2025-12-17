using BookingSystem.Application.DTOs.Schedule;

namespace BookingSystem.Application.Services.Interfaces;

public interface IScheduleService
{
    Task<List<ClassScheduleDto>> GetSchedulesAsync(string? countryCode = null, DateTime? startDate = null, DateTime? endDate = null);
    Task<ClassScheduleDto?> GetScheduleByIdAsync(int id);
    Task<ClassScheduleDto> CreateScheduleAsync(CreateScheduleDto request);
    Task<ClassScheduleDto> UpdateScheduleAsync(int id, UpdateScheduleDto request);
    Task<bool> DeleteScheduleAsync(int id);
}