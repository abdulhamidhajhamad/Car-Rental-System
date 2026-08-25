using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IRentalService, RentalService>();
        return services;
    }
}