using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SkyRouteTravel.Application.Bookings;
using SkyRouteTravel.Application.Flights;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFlightService, FlightService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IProviderService, ProviderService>();
        
        services.AddSingleton<IFlightProviderStrategyFactory, FlightProviderStrategyFactory>();
        
        services.AddValidatorsFromAssemblyContaining<BookingValidator>();
        
        return services;
    }
}
