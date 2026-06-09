namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Represents input details of a passenger to be added to a booking.
/// </summary>
public class PassengerRequestDto
{
    /// <summary>
    /// The full name of the passenger.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// The contact email address of the passenger.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The type of identification document provided. Valid values: 'Passport Number' or 'National ID'.
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// The alphanumeric document number. Validated contextually based on the DocumentType value.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;
}
