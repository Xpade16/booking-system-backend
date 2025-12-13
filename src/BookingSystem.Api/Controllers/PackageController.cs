using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Application.DTOs.Package;
using BookingSystem.Application.Services.Interfaces;
using BookingSystem.Domain.Exceptions;

namespace BookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PackageController : ControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ILogger<PackageController> _logger;

    public PackageController(IPackageService packageService, ILogger<PackageController> logger)
    {
        _packageService = packageService;
        _logger = logger;
    }

    /// <summary>
    /// Get all available packages for a specific country
    /// </summary>
    /// <param name="countryCode">Country code (e.g., SG, MY, TH, US)</param>
    /// <returns>List of available packages</returns>
    /// <response code="200">Returns list of packages</response>
    /// <response code="400">If country code is invalid</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<PackageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPackages([FromQuery] string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return BadRequest(new { message = "Country code is required" });
        }

        var packages = await _packageService.GetPackagesByCountryAsync(countryCode);
        return Ok(packages);
    }

    /// <summary>
    /// Purchase a package with credits
    /// </summary>
    /// <param name="request">Purchase request containing package ID and payment token</param>
    /// <returns>Purchase confirmation with package details</returns>
    /// <response code="200">Package purchased successfully</response>
    /// <response code="400">If payment fails or validation errors occur</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="404">If package not found</response>
    [HttpPost("purchase")]
    [Authorize]
    [ProducesResponseType(typeof(PurchasePackageResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PurchasePackage([FromBody] PurchasePackageRequestDto request)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _packageService.PurchasePackageAsync(userId, request);
            return Ok(response);
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
    /// Get all packages owned by the current user
    /// </summary>
    /// <returns>List of user's packages with remaining credits and expiry info</returns>
    /// <response code="200">Returns list of user packages</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpGet("my-packages")]
    [Authorize]
    [ProducesResponseType(typeof(List<UserPackageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyPackages()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var packages = await _packageService.GetUserPackagesAsync(userId);
        return Ok(packages);
    }
}