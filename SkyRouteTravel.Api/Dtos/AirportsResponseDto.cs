namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Response DTO containing a collection of airports.
/// </summary>
public class AirportsResponseDto
{
    /// <summary>
    /// The collection of airport details.
    /// </summary>
    public IEnumerable<AirportResponseDto> Airports { get; set; } = [];
}
