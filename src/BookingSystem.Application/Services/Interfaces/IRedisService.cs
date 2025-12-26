namespace BookingSystem.Application.Services.Interfaces;

public interface IRedisService
{
    Task<bool> TryDecrementSlotAsync(int classScheduleId);
    Task IncrementSlotAsync(int classScheduleId);
    Task<int> GetAvailableSlotsAsync(int classScheduleId);
    Task SetAvailableSlotsAsync(int classScheduleId, int slots);
    Task DeleteSlotKeyAsync(int classScheduleId);
    Task<bool> IsConnectedAsync();
}