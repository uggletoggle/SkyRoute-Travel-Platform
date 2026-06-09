namespace SkyRouteTravel.Application.Bookings;

public class Booking
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BookingReference { get; set; } = string.Empty;
    public string FlightId { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public int NumberOfPassengers { get; set; }
    public decimal PricePerPassenger { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsInternational { get; set; }
    public List<Passenger> Passengers { get; set; } = new();
}
