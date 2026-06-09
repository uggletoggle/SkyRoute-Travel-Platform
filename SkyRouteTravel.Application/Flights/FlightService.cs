namespace SkyRouteTravel.Application.Flights;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;

    public FlightService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    public Task<IEnumerable<Flight>> SearchFlightsAsync(string? origin, string? destination, string? cabinClass)
    {
        return _flightRepository.SearchFlightsAsync(origin, destination, cabinClass);
    }

    public Task<IEnumerable<Airport>> GetAirportsAsync()
    {
        return _flightRepository.GetAirportsAsync();
    }
}
