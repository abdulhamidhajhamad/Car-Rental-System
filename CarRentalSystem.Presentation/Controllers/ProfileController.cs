using System.Security.Claims;
using CarRentalSystem.Application.DTOs.Profile;
using CarRentalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfileResponseDto>> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var profile = await _profileService.GetProfileAsync(userId);
        return Ok(profile);
    }

    [HttpPut]
    public async Task<ActionResult<UserProfileResponseDto>> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var updatedProfile = await _profileService.UpdateProfileAsync(userId, dto);
        return Ok(updatedProfile);
    }
}