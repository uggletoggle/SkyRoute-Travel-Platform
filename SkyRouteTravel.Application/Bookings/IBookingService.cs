namespace SkyRouteTravel.Application.Bookings;

public interface IBookingService
{
    Task<Booking> BookFlightAsync(Booking booking);
    Task<Booking> GetBookingByReferenceAsync(string reference);
}
