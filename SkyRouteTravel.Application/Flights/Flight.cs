namespace SkyRouteTravel.Application.Flights;

public class Flight
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Provider { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    public string OriginAirportId { get; set; } = string.Empty;
    public Airport? OriginAirport { get; set; }
    public string DestinationAirportId { get; set; } = string.Empty;
    public Airport? DestinationAirport { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int DurationMinutes { get; set; }
    public string CabinClass { get; set; } = string.Empty;
    public decimal BaseFarePerPassenger { get; set; }
}
