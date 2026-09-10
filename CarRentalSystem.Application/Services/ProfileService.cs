using CarRentalSystem.Application.DTOs.Profile;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ValidationException = CarRentalSystem.Application.Exceptions.ValidationException;
namespace CarRentalSystem.Application.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IValidator<UpdateProfileDto> _validator;

    public ProfileService(
        UserManager<ApplicationUser> userManager,
        IValidator<UpdateProfileDto> validator)
    {
        _userManager = userManager;
        _validator = validator;
    }

    public async Task<UserProfileResponseDto> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User", userId);

        return MapToDto(user);
    }

    public async Task<UserProfileResponseDto> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User", userId);

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.DriversLicenseNumber = dto.DriversLicenseNumber;
        user.AddressLine1 = dto.AddressLine1;
        user.AddressLine2 = dto.AddressLine2;
        user.City = dto.City;
        user.Country = dto.Country;
        user.DateOfBirth = dto.DateOfBirth;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"Failed to update profile: {errors}");
        }

        return MapToDto(user);
    }

    private static UserProfileResponseDto MapToDto(ApplicationUser user)
    {
        return new UserProfileResponseDto(
            user.FirstName,
            user.LastName,
            user.Email!,
            user.PhoneNumber,
            user.DriversLicenseNumber,
            user.AddressLine1,
            user.AddressLine2,
            user.City,
            user.Country,
            user.DateOfBirth
        );
    }
}