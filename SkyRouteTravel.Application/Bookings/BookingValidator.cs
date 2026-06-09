using FluentValidation;
using SkyRouteTravel.Application.Flights;

namespace SkyRouteTravel.Application.Bookings;

public class BookingValidator : AbstractValidator<Booking>
{
    private readonly IFlightRepository _flightRepository;
    private readonly IBookingRepository _bookingRepository;

    public BookingValidator(IFlightRepository flightRepository, IBookingRepository bookingRepository)
    {
        _flightRepository = flightRepository;
        _bookingRepository = bookingRepository;

        RuleFor(b => b.FlightId)
            .NotEmpty().WithMessage("Flight ID is required.")
            .MustAsync(FlightExistsAsync).WithMessage("The specified flight does not exist.");

        RuleFor(b => b.NumberOfPassengers)
            .InclusiveBetween(1, 9).WithMessage("Number of passengers must be between 1 and 9.");

        RuleFor(b => b.Passengers)
            .NotEmpty().WithMessage("Passenger list cannot be empty.")
            .Must((booking, passengers) => passengers != null && passengers.Count == booking.NumberOfPassengers)
            .WithMessage(booking => $"The passenger details list count ({booking.Passengers?.Count ?? 0}) must match NumberOfPassengers ({booking.NumberOfPassengers}).")
            .Must(HaveUniqueDocumentNumbers).WithMessage("Duplicate identification numbers found. Each passenger must have a unique identification number.");

        RuleForEach(b => b.Passengers)
            .SetValidator(new PassengerValidator())
            .MustAsync(async (booking, passenger, cancellationToken) => 
                !await _bookingRepository.IsPassengerBookedOnFlightAsync(booking.FlightId, passenger.DocumentNumber))
            .WithMessage((booking, passenger) => 
                $"Passenger with identification number '{passenger.DocumentNumber}' has already booked this flight.");
    }

    private async Task<bool> FlightExistsAsync(string flightId, CancellationToken cancellationToken)
    {
        var flight = await _flightRepository.GetFlightByIdAsync(flightId);
        return flight != null;
    }

    private bool HaveUniqueDocumentNumbers(List<Passenger>? passengers)
    {
        if (passengers == null || passengers.Count == 0)
            return true;

        var documentNumbers = passengers
            .Select(p => p.DocumentNumber)
            .Where(dn => !string.IsNullOrWhiteSpace(dn))
            .ToList();

        return documentNumbers.Count == documentNumbers.Distinct(StringComparer.OrdinalIgnoreCase).Count();
    }
}
