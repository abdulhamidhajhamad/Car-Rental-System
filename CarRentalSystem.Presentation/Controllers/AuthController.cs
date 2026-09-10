using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register()
    {
        return Ok(new { message = "Register endpoint hit" });
    }

    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok(new { message = "Login endpoint hit" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logout endpoint hit" });
    }
}