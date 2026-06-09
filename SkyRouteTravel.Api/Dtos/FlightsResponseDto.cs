namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Response DTO containing a collection of flights.
/// </summary>
public class FlightsResponseDto
{
    /// <summary>
    /// The collection of flight details.
    /// </summary>
    public IEnumerable<FlightResponseDto> Flights { get; set; } = [];
}
