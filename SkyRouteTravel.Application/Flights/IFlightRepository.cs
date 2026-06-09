namespace SkyRouteTravel.Application.Flights;

public interface IFlightRepository
{
    Task<IEnumerable<Flight>> SearchFlightsAsync(string? origin, string? destination, string? cabinClass);
    Task<Flight?> GetFlightByIdAsync(string id);
    Task<IEnumerable<Airport>> GetAirportsAsync();
    Task<Airport?> GetAirportByIdAsync(string id);
}
