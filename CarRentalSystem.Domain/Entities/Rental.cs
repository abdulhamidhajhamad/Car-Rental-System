namespace CarRentalSystem.Domain.Entities;

public class Rental
{
    public int Id { get; private set; }
    public int CarId { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal TotalCost { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Car? Car { get; set; }
    public ApplicationUser? User { get; set; }

    public static Rental Create(int carId, string userId, DateTime startDate, DateTime endDate, decimal dailyRate)
    {
        int rentalDays = (endDate - startDate).Days;
        if (rentalDays == 0) rentalDays = 1;

        return new Rental
        {
            CarId = carId,
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate,
            TotalCost = rentalDays * dailyRate,
            IsCompleted = false
        };
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}