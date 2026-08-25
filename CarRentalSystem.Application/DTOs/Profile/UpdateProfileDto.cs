namespace CarRentalSystem.Application.DTOs.Profile;

public record UpdateProfileDto(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string DriversLicenseNumber,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string Country,
    DateTime? DateOfBirth
);