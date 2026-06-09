using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyRouteTravel.Application.Bookings;
using SkyRouteTravel.Application.Flights;
using SkyRouteTravel.Application.Providers;
using SkyRouteTravel.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace SkyRouteTravel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? "Data Source=SkyRouteTravel.db";
        
        services.AddDbContext<SkyRouteDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IFlightRepository, FlightRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddMemoryCache();
        services.AddScoped<ProviderRepository>();
        services.AddScoped<IProviderRepository>(sp => 
            new ProviderCachingRepositoryDecorator(
                sp.GetRequiredService<ProviderRepository>(),
                sp.GetRequiredService<IMemoryCache>()
            ));

        // Register AutoMapper
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DependencyInjection).Assembly));

        return services;
    }
}
