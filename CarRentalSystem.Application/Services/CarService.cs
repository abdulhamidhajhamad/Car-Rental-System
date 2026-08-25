using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<IEnumerable<CarResponseDto>> GetAllAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        return cars.Select(c => new CarResponseDto(c.Id, c.Make, c.Model, c.Year, c.DailyRate, c.LicensePlate, c.IsAvailable));
    }

    public async Task<IEnumerable<CarResponseDto>> GetAvailableCarsAsync()
    {
        var cars = await _carRepository.GetAvailableCarsAsync();
        return cars.Select(c => new CarResponseDto(c.Id, c.Make, c.Model, c.Year, c.DailyRate, c.LicensePlate, c.IsAvailable));
    }

    public async Task<CarResponseDto?> GetByIdAsync(int id)
    {
        var car = await _carRepository.GetByIdAsync(id);
        if (car == null) return null;

        return new CarResponseDto(car.Id, car.Make, car.Model, car.Year, car.DailyRate, car.LicensePlate, car.IsAvailable);
    }

    public async Task<CarResponseDto> CreateAsync(CreateCarDto dto)
    {
        var car = new Car
        {
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            DailyRate = dto.DailyRate,
            LicensePlate = dto.LicensePlate,
            IsAvailable = true
        };

        await _carRepository.AddAsync(car);

        return new CarResponseDto(car.Id, car.Make, car.Model, car.Year, car.DailyRate, car.LicensePlate, car.IsAvailable);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCarDto dto)
    {
        var car = await _carRepository.GetByIdAsync(id);
        if (car == null) return false;

        car.Make = dto.Make;
        car.Model = dto.Model;
        car.Year = dto.Year;
        car.DailyRate = dto.DailyRate;
        car.IsAvailable = dto.IsAvailable;

        await _carRepository.UpdateAsync(car);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var car = await _carRepository.GetByIdAsync(id);
        if (car == null) return false;

        await _carRepository.DeleteAsync(car);
        return true;
    }
}