namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Represents response details for an associated passenger.
/// </summary>
public class PassengerResponseDto
{
    /// <summary>
    /// The unique identifier of the passenger.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the booking this passenger belongs to.
    /// </summary>
    public string BookingId { get; set; } = string.Empty;

    /// <summary>
    /// The full name of the passenger.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// The contact email address of the passenger.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The type of identification document provided (e.g., 'Passport Number' or 'National ID').
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// The alphanumeric document number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;
}
