using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _carService.GetAllAsync();
        return Ok(cars);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    {
        var cars = await _carService.GetAvailableCarsAsync();
        return Ok(cars);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car == null) return NotFound(new { Message = $"Car with ID {id} was not found." });

        return Ok(car);
    }

    [HttpPost]
    [Authorize] 
    public async Task<IActionResult> Create([FromBody] CreateCarDto dto)
    {
        var createdCar = await _carService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdCar.Id }, createdCar);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCarDto dto)
    {
        var updated = await _carService.UpdateAsync(id, dto);
        if (!updated) return NotFound(new { Message = $"Car with ID {id} was not found." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _carService.DeleteAsync(id);
        if (!deleted) return NotFound(new { Message = $"Car with ID {id} was not found." });

        return NoContent();
    }
}