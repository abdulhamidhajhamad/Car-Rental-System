namespace CarRentalSystem.Application.DTOs.Rental;

public record RentalResponseDto(
    int Id,
    int CarId,
    string CarModel,
    string UserId,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalCost,
    bool IsCompleted
);