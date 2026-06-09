namespace SkyRouteTravel.Application.Exceptions;

public class FlightNotFoundException : NotFoundException
{
    public FlightNotFoundException(string flightId)
        : base("Flight", flightId)
    {
    }
}
