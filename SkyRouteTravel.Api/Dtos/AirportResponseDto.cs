namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Represents response details for an airport.
/// </summary>
public class AirportResponseDto
{
    /// <summary>
    /// The unique identifier (GUID) of the airport.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The 3-letter IATA code of the airport (e.g., JFK, LAX).
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// The full name of the airport (e.g., John F. Kennedy International Airport).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The city where the airport is located.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The country where the airport is located.
    /// </summary>
    public string Country { get; set; } = string.Empty;
}
