using CarRentalSystem.Application.DTOs.Rental;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Domain.Entities;
using FluentValidation;
using ValidationException = CarRentalSystem.Application.Exceptions.ValidationException;

namespace CarRentalSystem.Application.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICarRepository _carRepository;
    private readonly IValidator<CreateRentalDto> _validator;

    public RentalService(
        IRentalRepository rentalRepository,
        ICarRepository carRepository,
        IValidator<CreateRentalDto> validator)
    {
        _rentalRepository = rentalRepository;
        _carRepository = carRepository;
        _validator = validator;
    }

    public async Task<RentalResponseDto> CreateRentalAsync(string userId, CreateRentalDto dto)
    {
        // 1. التحقق من المدخلات (Application Level Validation)
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // 2. جلب السيارة والتأكد من وجودها
        var car = await _carRepository.GetByIdAsync(dto.CarId);
        if (car == null)
            throw new NotFoundException(nameof(Car), dto.CarId);

        // 3. التحقق من عدم وجود تضارب في التواريخ
        bool isOverlapping = await _rentalRepository.HasOverlapAsync(dto.CarId, dto.StartDate, dto.EndDate);
        if (isOverlapping)
            throw new BadRequestException("The selected car is already booked for the specified dates.");

        // 4. استدعاء منطق البزنس من الـ Domain بإنشاء الـ Entity وتمرير البيانات للـ Factory Method
        var rental = Rental.Create(dto.CarId, userId, dto.StartDate, dto.EndDate, car.DailyRate);

        // 5. الحفظ في قاعدة البيانات
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
            r.Id,
            r.CarId,
            r.Car != null ? $"{r.Car.Make} {r.Car.Model}" : "N/A",
            r.UserId,
            r.StartDate,
            r.EndDate,
            r.TotalCost,
            r.IsCompleted));
    }

    public async Task<IEnumerable<RentalResponseDto>> GetAllRentalsAsync()
    {
        var rentals = await _rentalRepository.GetAllAsync();
        return rentals.Select(r => new RentalResponseDto(
            r.Id,
            r.CarId,
            r.Car != null ? $"{r.Car.Make} {r.Car.Model}" : "N/A",
            r.UserId,
            r.StartDate,
            r.EndDate,
            r.TotalCost,
            r.IsCompleted));
    }

    public async Task<bool> CompleteRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental == null)
            throw new NotFoundException(nameof(Rental), rentalId);

        if (rental.IsCompleted)
            throw new BadRequestException("This rental is already completed.");

        rental.Complete();

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