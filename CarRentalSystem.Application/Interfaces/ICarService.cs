using CarRentalSystem.Application.DTOs.Car;

namespace CarRentalSystem.Application.Interfaces;

public interface ICarService
{
    Task<IEnumerable<CarResponseDto>> GetAllAsync();
    Task<IEnumerable<CarResponseDto>> GetAvailableCarsAsync();
    Task<CarResponseDto?> GetByIdAsync(int id);
    Task<CarResponseDto> CreateCarAsync(CreateCarDto dto);
    Task<bool> UpdateCarAsync(int id, UpdateCarDto dto);
    Task<bool> DeleteCarAsync(int id);
    Task<IEnumerable<CarResponseDto>> SearchCarsAsync(CarSearchDto searchDto);
}