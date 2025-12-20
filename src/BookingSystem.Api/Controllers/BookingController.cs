using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Application.DTOs.Booking;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Exceptions;

namespace BookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingController> _logger;

    public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Book a class
    /// </summary>
    /// <param name="classScheduleId">The ID of the class to book</param>
    [HttpPost("book/{classScheduleId}")]
    [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BookClass(int classScheduleId)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _bookingService.BookClassAsync(userId, classScheduleId);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InsufficientCreditsException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (BookingException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cancel a booking
    /// </summary>
    /// <param name="bookingId">The ID of the booking to cancel</param>
    [HttpDelete("cancel/{bookingId}")]
    [ProducesResponseType(typeof(CancelBookingResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _bookingService.CancelBookingAsync(userId, bookingId);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BookingException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all bookings for the current user
    /// </summary>
    /// <param name="status">Optional filter by status (Confirmed, CheckedIn, Cancelled)</param>
    [HttpGet("my-bookings")]
    [ProducesResponseType(typeof(List<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyBookings([FromQuery] string? status = null)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var bookings = await _bookingService.GetUserBookingsAsync(userId, status);
        return Ok(bookings);
    }

    /// <summary>
    /// Check in to a booked class
    /// </summary>
    /// <param name="bookingId">The ID of the booking to check in to</param>
    [HttpPost("checkin/{bookingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckIn(int bookingId)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _bookingService.CheckInAsync(userId, bookingId);
            return Ok(new { message = "Checked in successfully!" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BookingException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}