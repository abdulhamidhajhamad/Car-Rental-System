namespace CarRentalSystem.Application.DTOs.Profile;

public record UserProfileResponseDto(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string DriversLicenseNumber,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string Country,
    DateTime? DateOfBirth
);