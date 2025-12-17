using Microsoft.AspNetCore.Mvc;
using BookingSystem.Application.DTOs.Schedule;
using BookingSystem.Application.Services.Interfaces;

namespace BookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly ILogger<ScheduleController> _logger;

    public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger)
    {
        _scheduleService = scheduleService;
        _logger = logger;
    }

    /// <summary>
    /// Get all active class schedules with optional filters
    /// </summary>
    /// <param name="countryCode">Filter by country code (e.g., SG)</param>
    /// <param name="startDate">Filter classes starting from this date</param>
    /// <param name="endDate">Filter classes until this date</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClassScheduleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedules(
        [FromQuery] string? countryCode = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var schedules = await _scheduleService.GetSchedulesAsync(countryCode, startDate, endDate);
        return Ok(schedules);
    }

    /// <summary>
    /// Get a specific class schedule by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClassScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScheduleById(int id)
    {
        var schedule = await _scheduleService.GetScheduleByIdAsync(id);
        
        if (schedule == null)
        {
            return NotFound(new { message = "Class schedule not found" });
        }

        return Ok(schedule);
    }
}