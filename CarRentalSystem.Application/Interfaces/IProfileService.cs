using CarRentalSystem.Application.DTOs.Profile;

namespace CarRentalSystem.Application.Interfaces;

public interface IProfileService
{
    Task<UserProfileResponseDto> GetProfileAsync(string userId);
    Task<UserProfileResponseDto> UpdateProfileAsync(string userId, UpdateProfileDto dto);
}