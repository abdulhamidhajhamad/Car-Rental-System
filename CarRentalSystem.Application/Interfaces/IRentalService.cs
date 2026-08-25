using CarRentalSystem.Application.DTOs.Rental;

namespace CarRentalSystem.Application.Interfaces;

public interface IRentalService
{
    Task<RentalResponseDto> CreateRentalAsync(string userId, CreateRentalDto dto);
    Task<IEnumerable<RentalResponseDto>> GetMyRentalsAsync(string userId);
    Task<IEnumerable<RentalResponseDto>> GetAllRentalsAsync();
    Task<bool> CompleteRentalAsync(int rentalId);
}