using FluentValidation;
using SkyRouteTravel.Application.Exceptions;
using SkyRouteTravel.Application.Flights;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Application.Bookings;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IFlightProviderStrategyFactory _strategyFactory;
    private readonly IValidator<Booking> _validator;

    public BookingService(
        IBookingRepository bookingRepository,
        IFlightRepository flightRepository,
        IFlightProviderStrategyFactory strategyFactory,
        IValidator<Booking> validator)
    {
        _bookingRepository = bookingRepository;
        _flightRepository = flightRepository;
        _strategyFactory = strategyFactory;
        _validator = validator;
    }

    public async Task<Booking> BookFlightAsync(Booking booking)
    {
        var validationResult = await _validator.ValidateAsync(booking);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var flight = await _flightRepository.GetFlightByIdAsync(booking.FlightId) 
            ?? throw new FlightNotFoundException(booking.FlightId);

        var strategy = _strategyFactory.GetStrategy(flight.Provider);
        
        booking.PricePerPassenger = strategy.CalculateFinalPrice(flight.BaseFarePerPassenger);
        booking.TotalPrice = booking.PricePerPassenger * booking.NumberOfPassengers;
        flight.OriginAirport ??= await _flightRepository.GetAirportByIdAsync(flight.OriginAirportId);
        flight.DestinationAirport ??= await _flightRepository.GetAirportByIdAsync(flight.DestinationAirportId);

        if (flight.OriginAirport != null && flight.DestinationAirport != null)
        {
            booking.IsInternational = !string.Equals(
                flight.OriginAirport.Country,
                flight.DestinationAirport.Country,
                StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            booking.IsInternational = false;
        }

        booking.BookingReference = "SR-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
        booking.BookingDate = DateTime.UtcNow;

        if (booking.Passengers != null)
        {
            foreach (var passenger in booking.Passengers)
            {
                if (string.IsNullOrWhiteSpace(passenger.Id))
                {
                    passenger.Id = Guid.NewGuid().ToString();
                }
                passenger.BookingId = booking.Id;
            }
        }

        return await _bookingRepository.CreateBookingAsync(booking);
    }

    public async Task<Booking> GetBookingByReferenceAsync(string reference)
    {
        var booking = await _bookingRepository.GetBookingByReferenceAsync(reference)
            ?? throw new BookingNotFoundException(reference);

        return booking;
    }
}
