using CarRentalSystem.Application.DTOs.Car;

namespace CarRentalSystem.Application.Interfaces;

public interface ICarService
{
    Task<IEnumerable<CarResponseDto>> GetAllAsync();
    Task<IEnumerable<CarResponseDto>> GetAvailableCarsAsync();
    Task<CarResponseDto?> GetByIdAsync(int id);
    Task<CarResponseDto> CreateAsync(CreateCarDto dto);
    Task<bool> UpdateAsync(int id, UpdateCarDto dto);
    Task<bool> DeleteAsync(int id);
}