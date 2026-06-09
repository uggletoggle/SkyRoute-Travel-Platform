using System.Collections.Generic;

namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Represents a request body to book a flight for one or more passengers.
/// </summary>
public class BookFlightRequestDto
{
    /// <summary>
    /// The unique identifier of the flight to book.
    /// </summary>
    public string FlightId { get; set; } = string.Empty;

    /// <summary>
    /// The total number of passengers. Must match the length of the Passengers list (between 1 and 9).
    /// </summary>
    public int NumberOfPassengers { get; set; }

    /// <summary>
    /// The list of passenger details.
    /// </summary>
    public List<PassengerRequestDto> Passengers { get; set; } = new();
}
