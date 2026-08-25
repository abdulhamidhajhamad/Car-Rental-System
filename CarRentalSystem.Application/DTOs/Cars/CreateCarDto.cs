namespace CarRentalSystem.Application.DTOs.Car;

public record CreateCarDto(
    string Make,
    string Model,
    int Year,
    decimal DailyRate,
    string LicensePlate
);