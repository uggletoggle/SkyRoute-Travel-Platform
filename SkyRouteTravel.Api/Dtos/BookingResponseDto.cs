using System;
using System.Collections.Generic;

namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Represents response details for a booking, including the associated flight and passengers.
/// </summary>
public class BookingResponseDto
{
    /// <summary>
    /// The unique identifier (GUID) of the booking.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The unique booking reference code (e.g., SR-A2B3C4).
    /// </summary>
    public string BookingReference { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the booked flight.
    /// </summary>
    public string FlightId { get; set; } = string.Empty;

    /// <summary>
    /// The details of the booked flight.
    /// </summary>
    public FlightResponseDto? Flight { get; set; }

    /// <summary>
    /// The date and time when the booking was made.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// The total number of passengers.
    /// </summary>
    public int NumberOfPassengers { get; set; }

    /// <summary>
    /// The price per individual passenger, computed according to the flight provider strategy markup rules.
    /// </summary>
    public decimal PricePerPassenger { get; set; }

    /// <summary>
    /// The total price of the booking (PricePerPassenger * NumberOfPassengers).
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Indicates whether the booking is for an international flight (crossing countries).
    /// </summary>
    public bool IsInternational { get; set; }

    /// <summary>
    /// The list of passengers registered under this booking.
    /// </summary>
    public List<PassengerResponseDto> Passengers { get; set; } = new();
}
