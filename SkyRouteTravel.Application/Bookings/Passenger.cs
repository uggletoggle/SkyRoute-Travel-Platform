namespace SkyRouteTravel.Application.Bookings;

public class Passenger
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BookingId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
}
