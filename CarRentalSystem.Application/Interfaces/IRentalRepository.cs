using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Interfaces;

public interface IRentalRepository
{
    Task<IEnumerable<Rental>> GetAllAsync();
    Task<Rental?> GetByIdAsync(int id);
    Task<IEnumerable<Rental>> GetByUserIdAsync(string userId);
    Task AddAsync(Rental rental);
    Task UpdateAsync(Rental rental);
    Task<bool> HasOverlapAsync(int carId, DateTime startDate, DateTime endDate);
}