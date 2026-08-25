namespace CarRentalSystem.Application.DTOs.Rentals;

public class RentalDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string CarModel { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalCost { get; set; }
    public string Status { get; set; } = string.Empty;
}