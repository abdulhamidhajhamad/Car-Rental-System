using CarRentalSystem.Application.DTOs.Rental;
using FluentValidation;

namespace CarRentalSystem.Application.Validators;

public class CreateRentalDtoValidator : AbstractValidator<CreateRentalDto>
{
    public CreateRentalDtoValidator()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0).WithMessage("Car ID must be greater than 0.");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after the start date.");
    }
}