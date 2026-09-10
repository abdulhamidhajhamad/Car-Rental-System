using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateRental()
    {
        return Ok(new { message = "Create rental endpoint hit" });
    }

    [HttpGet("my-rentals")]
    public IActionResult GetMyRentals()
    {
        return Ok(new { message = "Get my rentals endpoint hit" });
    }

    [HttpGet("{id}")]
    public IActionResult GetRentalById(int id)
    {
        return Ok(new { message = $"Get rental {id} endpoint hit" });
    }

    [HttpPut("{id}/cancel")]
    public IActionResult CancelRental(int id)
    {
        return Ok(new { message = $"Cancel rental {id} endpoint hit" });
    }
}