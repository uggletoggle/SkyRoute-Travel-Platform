namespace SkyRouteTravel.Application.Exceptions;

public class BookingNotFoundException : NotFoundException
{
    public BookingNotFoundException(string reference)
        : base("Booking", reference)
    {
    }
}
