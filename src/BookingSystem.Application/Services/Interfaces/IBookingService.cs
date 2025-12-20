using BookingSystem.Application.DTOs.Booking;

namespace BookingSystem.Application.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponseDto> BookClassAsync(int userId, int classScheduleId);
    Task<CancelBookingResponseDto> CancelBookingAsync(int userId, int bookingId);
    Task<List<BookingDto>> GetUserBookingsAsync(int userId, string? status = null);
    Task<bool> CheckInAsync(int userId, int bookingId);
}