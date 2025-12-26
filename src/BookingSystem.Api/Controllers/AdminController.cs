using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Application.DTOs.Schedule;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Exceptions;

namespace BookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IScheduleService scheduleService, ILogger<AdminController> logger)
    {
        _scheduleService = scheduleService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new class schedule (Admin only)
    /// </summary>
    [HttpPost("schedules")]
    [ProducesResponseType(typeof(ClassScheduleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleDto request)
    {
        try
        {
            var schedule = await _scheduleService.CreateScheduleAsync(request);
            return CreatedAtAction(
                nameof(ScheduleController.GetScheduleById),
                "Schedule",
                new { id = schedule.Id },
                schedule);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing class schedule (Admin only)
    /// </summary>
    [HttpPut("schedules/{id}")]
    [ProducesResponseType(typeof(ClassScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSchedule(int id, [FromBody] UpdateScheduleDto request)
    {
        try
        {
            var schedule = await _scheduleService.UpdateScheduleAsync(id, request);
            return Ok(schedule);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a class schedule (soft delete - Admin only)
    /// </summary>
    [HttpDelete("schedules/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        try
        {
            await _scheduleService.DeleteScheduleAsync(id);
            return Ok(new { message = "Schedule deleted successfully" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Check Redis connection status (Admin only)
    /// </summary>
    [HttpGet("redis/health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CheckRedisHealth()
    {
        var redisService = HttpContext.RequestServices.GetRequiredService<IRedisService>();
        var isConnected = await redisService.IsConnectedAsync();

        if (isConnected)
        {
            return Ok(new
            {
                status = "healthy",
                message = "Redis is connected and responding",
                timestamp = DateTime.UtcNow
            });
        }
        else
        {
            return StatusCode(503, new
            {
                status = "unhealthy",
                message = "Redis is not responding",
                timestamp = DateTime.UtcNow
            });
        }
    }
}