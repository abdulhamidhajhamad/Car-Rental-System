using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllCars()
    {
        // TODO: جلب جميع السيارات (مع إمكانية الفلترة)
        return Ok(new { message = "Get all cars endpoint hit" });
    }

    [HttpGet("{id}")]
    public IActionResult GetCarById(int id)
    {
        // TODO: جلب تفاصيل سيارة معينة
        return Ok(new { message = $"Get car {id} endpoint hit" });
    }

    [HttpPost]
    public IActionResult CreateCar()
    {
        // TODO: إضافة سيارة جديدة (للمدير فقط)
        return Ok(new { message = "Create car endpoint hit" });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCar(int id)
    {
        // TODO: تعديل بيانات سيارة (للمدير فقط)
        return Ok(new { message = $"Update car {id} endpoint hit" });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCar(int id)
    {
        // TODO: حذف أو إيقاف سيارة (للمدير فقط)
        return Ok(new { message = $"Delete car {id} endpoint hit" });
    }
}