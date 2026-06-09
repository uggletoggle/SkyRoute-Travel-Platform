using System;

namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Represents response details for a flight, including origin and destination airport details.
/// </summary>
public class FlightResponseDto
{
    /// <summary>
    /// The unique identifier of the flight.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The airline/provider supplying this flight (e.g., GlobalAir, BudgetWings).
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// The flight designator number (e.g., GA-101).
    /// </summary>
    public string FlightNumber { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the departure airport.
    /// </summary>
    public string OriginAirportId { get; set; } = string.Empty;

    /// <summary>
    /// The departure airport details.
    /// </summary>
    public AirportResponseDto? OriginAirport { get; set; }

    /// <summary>
    /// The identifier of the arrival airport.
    /// </summary>
    public string DestinationAirportId { get; set; } = string.Empty;

    /// <summary>
    /// The arrival airport details.
    /// </summary>
    public AirportResponseDto? DestinationAirport { get; set; }

    /// <summary>
    /// The scheduled departure date and time in ISO 8601 format.
    /// </summary>
    public DateTime DepartureTime { get; set; }

    /// <summary>
    /// The scheduled arrival date and time in ISO 8601 format.
    /// </summary>
    public DateTime ArrivalTime { get; set; }

    /// <summary>
    /// The flight duration in minutes.
    /// </summary>
    public int DurationMinutes { get; set; }

    /// <summary>
    /// The cabin class associated with this flight (e.g., Economy, Business, First Class).
    /// </summary>
    public string CabinClass { get; set; } = string.Empty;

    /// <summary>
    /// The base fare price per individual passenger.
    /// </summary>
    public decimal BaseFarePerPassenger { get; set; }

    /// <summary>
    /// The final price per passenger after applying provider-specific pricing strategies.
    /// </summary>
    public decimal FinalPrice { get; set; }
}
