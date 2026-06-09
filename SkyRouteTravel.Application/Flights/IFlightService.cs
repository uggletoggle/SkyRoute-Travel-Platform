namespace SkyRouteTravel.Application.Flights;

public interface IFlightService
{
    Task<IEnumerable<Flight>> SearchFlightsAsync(string? origin, string? destination, string? cabinClass);
    Task<IEnumerable<Airport>> GetAirportsAsync();
}
