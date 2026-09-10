namespace CarRentalSystem.Application.DTOs.Car;

public record CarResponseDto(
    int Id,
    string Make,
    string Model,
    int Year,
    decimal DailyRate,
    string LicensePlate,
    bool IsAvailable
);