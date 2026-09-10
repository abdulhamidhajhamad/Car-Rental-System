using CarRentalSystem.Application.DTOs.Rental;
using CarRentalSystem.Application.Exceptions;
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
        // 1. تشغيل الـ Validator
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // 2. التحقق من وجود السيارة
        var car = await _carRepository.GetByIdAsync(dto.CarId);
        if (car == null)
            throw new NotFoundException(nameof(Car), dto.CarId);

        // 3. التحقق من تضارب التواريخ مع حجوزات سابقة
        bool isOverlapping = await _rentalRepository.HasOverlapAsync(dto.CarId, dto.StartDate, dto.EndDate);
        if (isOverlapping)
            throw new BadRequestException("The selected car is already booked for the specified dates.");

        // 4. إنشاء الحجز
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
        if (rental == null)
            throw new NotFoundException(nameof(Rental), rentalId);

        if (rental.IsCompleted)
            throw new BadRequestException("This rental is already completed.");

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