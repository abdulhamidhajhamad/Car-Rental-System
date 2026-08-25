using System.Security.Claims;
using CarRentalSystem.Application.DTOs.Rental;
using CarRentalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpPost]
    public async Task<IActionResult> BookCar([FromBody] CreateRentalDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        try
        {
            var result = await _rentalService.CreateRentalAsync(userId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("my-rentals")]
    public async Task<IActionResult> GetMyRentals()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var rentals = await _rentalService.GetMyRentalsAsync(userId);
        return Ok(rentals);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rentals = await _rentalService.GetAllRentalsAsync();
        return Ok(rentals);
    }

    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> CompleteRental(int id)
    {
        var success = await _rentalService.CompleteRentalAsync(id);
        if (!success) return BadRequest(new { Message = "Unable to complete rental. Please verify the rental ID." });

        return Ok(new { Message = "Rental completed and car returned successfully." });
    }
}