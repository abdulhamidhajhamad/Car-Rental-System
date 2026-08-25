namespace CarRentalSystem.Domain.Entities;
public class Car
{
    public int Id { get; set; }
    public string Make { get; set; } = string.Empty;      
    public string Model { get; set; } = string.Empty;    
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string Location { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}