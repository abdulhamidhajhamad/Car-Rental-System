using CarRentalSystem.Application.DTOs.Rental;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICarRepository _carRepository;

    public RentalService(IRentalRepository rentalRepository, ICarRepository carRepository)
    {
        _rentalRepository = rentalRepository;
        _carRepository = carRepository;
    }

    public async Task<RentalResponseDto> CreateRentalAsync(string userId, CreateRentalDto dto)
    {
        if (dto.EndDate <= dto.StartDate)
            throw new ArgumentException("End date must be after start date.");

        var car = await _carRepository.GetByIdAsync(dto.CarId);
        if (car == null || !car.IsAvailable)
            throw new InvalidOperationException("The selected car is currently not available for booking.");

        int rentalDays = (dto.EndDate - dto.StartDate).Days;
        if (rentalDays == 0) rentalDays = 1;

        decimal totalCost = rentalDays * car.DailyRate;

        var rental = new Rental
        {
            CarId = dto.CarId,
            UserId = userId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            TotalCost = totalCost,
            IsCompleted = false
        };

        car.IsAvailable = false;
        await _carRepository.UpdateAsync(car);
        await _rentalRepository.AddAsync(rental);

        return new RentalResponseDto(
            rental.Id,
            car.Id,
            $"{car.Make} {car.Model}",
            userId,
            rental.StartDate,
            rental.EndDate,
            rental.TotalCost,
            rental.IsCompleted
        );
    }

    public async Task<IEnumerable<RentalResponseDto>> GetMyRentalsAsync(string userId)
    {
        var rentals = await _rentalRepository.GetByUserIdAsync(userId);
        return rentals.Select(r => new RentalResponseDto(
            r.Id, r.CarId, $"{r.Car.Make} {r.Car.Model}", r.UserId, r.StartDate, r.EndDate, r.TotalCost, r.IsCompleted));
    }

    public async Task<IEnumerable<RentalResponseDto>> GetAllRentalsAsync()
    {
        var rentals = await _rentalRepository.GetAllAsync();
        return rentals.Select(r => new RentalResponseDto(
            r.Id, r.CarId, $"{r.Car.Make} {r.Car.Model}", r.UserId, r.StartDate, r.EndDate, r.TotalCost, r.IsCompleted));
    }

    public async Task<bool> CompleteRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental == null || rental.IsCompleted) return false;

        rental.IsCompleted = true;
        
        var car = await _carRepository.GetByIdAsync(rental.CarId);
        if (car != null)
        {
            car.IsAvailable = true;
            await _carRepository.UpdateAsync(car);
        }

        await _rentalRepository.UpdateAsync(rental);
        return true;
    }
}