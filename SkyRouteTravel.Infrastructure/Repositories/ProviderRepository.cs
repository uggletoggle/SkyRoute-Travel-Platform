using Microsoft.EntityFrameworkCore;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Infrastructure.Repositories;

public class ProviderRepository(SkyRouteDbContext context) : IProviderRepository
{
    private readonly SkyRouteDbContext _context = context;

    public async Task<IEnumerable<string>> GetProvidersAsync()
    {
        var dbProviders = await _context.Flights
            .Select(f => f.Provider)
            .Distinct()
            .ToListAsync();

        return dbProviders.Any() ? dbProviders : ["GlobalAir", "BudgetWings"];
    }
}
