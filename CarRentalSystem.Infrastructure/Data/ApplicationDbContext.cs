using CarRentalSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            entity.Property(u => u.DriversLicenseNumber).IsRequired().HasMaxLength(30);
            entity.Property(u => u.AddressLine1).IsRequired().HasMaxLength(150);
            entity.Property(u => u.City).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Country).IsRequired().HasMaxLength(50);
        });

        builder.Entity<Car>(entity =>
        {
            entity.Property(c => c.Make).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Model).IsRequired().HasMaxLength(50);
            entity.Property(c => c.DailyRate).HasColumnType("decimal(18,2)");
        });
    }
}