using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCars([FromQuery] CarSearchDto searchDto)
    {
        var cars = await _carService.SearchCarsAsync(searchDto);
        return Ok(cars);
    }

    // متاح فقط للمستخدمين برول Admin
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCar([FromBody] CreateCarDto dto)
    {
        var car = await _carService.CreateCarAsync(dto);
        return Ok(car);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarDto dto)
    {
        await _carService.UpdateCarAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        await _carService.DeleteCarAsync(id);
        return NoContent();
    }
}