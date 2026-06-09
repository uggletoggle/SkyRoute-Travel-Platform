using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkyRouteTravel.Application.Flights;

namespace SkyRouteTravel.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly SkyRouteDbContext _context;
    private readonly IMapper _mapper;

    public FlightRepository(SkyRouteDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Flight>> SearchFlightsAsync(string? origin, string? destination, string? cabinClass)
    {
        var query = _context.Flights
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(origin))
        {
            var originLower = origin.ToLowerInvariant();
            query = query.Where(f =>
                f.OriginAirport!.Code.ToLower() == originLower ||
                f.OriginAirport.City.ToLower() == originLower);
        }

        if (!string.IsNullOrWhiteSpace(destination))
        {
            var destinationLower = destination.ToLowerInvariant();
            query = query.Where(f =>
                f.DestinationAirport!.Code.ToLower() == destinationLower ||
                f.DestinationAirport.City.ToLower() == destinationLower);
        }

        if (!string.IsNullOrWhiteSpace(cabinClass))
        {
            var cabinLower = cabinClass.ToLowerInvariant();
            query = query.Where(f => f.CabinClass.ToLower() == cabinLower);
        }

        var dbFlights = await query.ToListAsync();
        return _mapper.Map<IEnumerable<Flight>>(dbFlights);
    }

    public async Task<Flight?> GetFlightByIdAsync(string id)
    {
        var dbFlight = await _context.Flights
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .FirstOrDefaultAsync(f => f.Id == id);

        return _mapper.Map<Flight?>(dbFlight);
    }

    public async Task<IEnumerable<Airport>> GetAirportsAsync()
    {
        var dbAirports = await _context.Airports.ToListAsync();
        return _mapper.Map<IEnumerable<Airport>>(dbAirports);
    }

    public async Task<Airport?> GetAirportByIdAsync(string id)
    {
        var dbAirport = await _context.Airports.FindAsync(id);
        return _mapper.Map<Airport?>(dbAirport);
    }
}
