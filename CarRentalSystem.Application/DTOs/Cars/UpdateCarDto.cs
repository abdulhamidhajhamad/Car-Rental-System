namespace CarRentalSystem.Application.DTOs.Car;

public record UpdateCarDto(
    string Make,
    string Model,
    int Year,
    decimal DailyRate,
    bool IsAvailable
);