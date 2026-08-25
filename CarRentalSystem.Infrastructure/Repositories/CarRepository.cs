using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CarRentalSystem.Application.DTOs.Car;
namespace CarRentalSystem.Infrastructure.Repositories;

public class CarRepository : ICarRepository
{
    private readonly ApplicationDbContext _context;

    public CarRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        return await _context.Cars.ToListAsync();
    }

    public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
    {
        return await _context.Cars.Where(c => c.IsAvailable).ToListAsync();
    }

    public async Task<Car?> GetByIdAsync(int id)
    {
        return await _context.Cars.FindAsync(id);
    }

    public async Task AddAsync(Car car)
    {
        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Car car)
    {
        _context.Cars.Update(car);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Car car)
    {
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Car>> SearchAvailableCarsAsync(CarSearchDto searchDto)
    {
        var query = _context.Cars.AsQueryable();

        if (searchDto.MinPrice.HasValue)
            query = query.Where(c => c.DailyRate >= searchDto.MinPrice.Value);

        if (searchDto.MaxPrice.HasValue)
            query = query.Where(c => c.DailyRate <= searchDto.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(searchDto.Make))
            query = query.Where(c => c.Make.Contains(searchDto.Make));

        if (!string.IsNullOrWhiteSpace(searchDto.Model))
            query = query.Where(c => c.Model.Contains(searchDto.Model));

        if (searchDto.StartDate.HasValue && searchDto.EndDate.HasValue)
        {
            var start = searchDto.StartDate.Value;
            var end = searchDto.EndDate.Value;

            query = query.Where(c => !_context.Rentals.Any(r =>
                r.CarId == c.Id &&
                !r.IsCompleted &&
                r.StartDate < end &&
                r.EndDate > start));
        }
        else
        {
            query = query.Where(c => c.IsAvailable);
        }

        return await query.ToListAsync();
    }
}