using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Interfaces;

public interface ICarRepository
{
    Task<IEnumerable<Car>> GetAllAsync();
    Task<IEnumerable<Car>> GetAvailableCarsAsync();
    Task<Car?> GetByIdAsync(int id);
    Task AddAsync(Car car);
    Task UpdateAsync(Car car);
    Task DeleteAsync(Car car);
    Task<IEnumerable<Car>> SearchAvailableCarsAsync(CarSearchDto searchDto);
}