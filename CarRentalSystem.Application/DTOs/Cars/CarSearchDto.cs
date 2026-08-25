namespace CarRentalSystem.Application.DTOs.Car;

public record CarSearchDto(
    DateTime? StartDate,
    DateTime? EndDate,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? Make,
    string? Model
);