namespace CarRentalSystem.Application.DTOs.Rental;

public record CreateRentalDto(
    int CarId,
    DateTime StartDate,
    DateTime EndDate
);