using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Exceptions;
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
        if (car == null)
            throw new NotFoundException(nameof(Car), id);

        return new CarResponseDto(car.Id, car.Make, car.Model, car.Year, car.DailyRate, car.LicensePlate, car.IsAvailable);
    }

    public async Task<CarResponseDto> CreateCarAsync(CreateCarDto dto)
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

    public async Task<bool> UpdateCarAsync(int id, UpdateCarDto dto)
    {
        var car = await _carRepository.GetByIdAsync(id);
        if (car == null)
            throw new NotFoundException(nameof(Car), id);

        car.Make = dto.Make;
        car.Model = dto.Model;
        car.Year = dto.Year;
        car.DailyRate = dto.DailyRate;
        car.IsAvailable = dto.IsAvailable;

        await _carRepository.UpdateAsync(car);
        return true;
    }

    public async Task<bool> DeleteCarAsync(int id)
    {
        var car = await _carRepository.GetByIdAsync(id);
        if (car == null)
            throw new NotFoundException(nameof(Car), id);

        await _carRepository.DeleteAsync(car);
        return true;
    }

    public async Task<IEnumerable<CarResponseDto>> SearchCarsAsync(CarSearchDto searchDto)
    {
        if (searchDto.StartDate.HasValue && searchDto.EndDate.HasValue)
        {
            if (searchDto.EndDate <= searchDto.StartDate)
                throw new BadRequestException("End date must be after start date.");
        }

        var cars = await _carRepository.SearchAvailableCarsAsync(searchDto);

        return cars.Select(c => new CarResponseDto(
            c.Id, c.Make, c.Model, c.Year, c.DailyRate, c.LicensePlate, c.IsAvailable));
    }
}