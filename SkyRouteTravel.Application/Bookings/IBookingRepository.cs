namespace SkyRouteTravel.Application.Bookings;

public interface IBookingRepository
{
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<Booking?> GetBookingByReferenceAsync(string reference);
    Task<bool> IsPassengerBookedOnFlightAsync(string flightId, string documentNumber);
}
