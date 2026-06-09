using FluentAssertions;
using FluentValidation;
using NSubstitute;
using SkyRouteTravel.Application.Bookings;
using SkyRouteTravel.Application.Exceptions;
using SkyRouteTravel.Application.Flights;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Tests.Bookings;

public class BookingServiceTests
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IFlightProviderStrategyFactory _strategyFactory;
    private readonly IValidator<Booking> _validator;
    private readonly BookingService _sut;

    public BookingServiceTests()
    {
        _bookingRepository = Substitute.For<IBookingRepository>();
        _flightRepository = Substitute.For<IFlightRepository>();
        _strategyFactory = Substitute.For<IFlightProviderStrategyFactory>();
        _validator = Substitute.For<IValidator<Booking>>();
        _sut = new BookingService(_bookingRepository, _flightRepository, _strategyFactory, _validator);
    }

    // --- BookFlightAsync ---

    [Fact]
    public async Task BookFlightAsync_ValidBooking_CreatesAndReturnsBooking()
    {
        var flight = BuildFlight("f1", "GlobalAir", "US", "US", 100m);
        var booking = BuildBooking("f1");
        var strategy = Substitute.For<IFlightProviderStrategy>();
        strategy.CalculateFinalPrice(100m).Returns(115m);

        SetValidatorValid();
        _flightRepository.GetFlightByIdAsync("f1").Returns(flight);
        _flightRepository.GetAirportByIdAsync(flight.OriginAirportId).Returns(flight.OriginAirport);
        _flightRepository.GetAirportByIdAsync(flight.DestinationAirportId).Returns(flight.DestinationAirport);
        _strategyFactory.GetStrategy("GlobalAir").Returns(strategy);
        _bookingRepository.CreateBookingAsync(Arg.Any<Booking>()).Returns(ci => ci.Arg<Booking>());

        var result = await _sut.BookFlightAsync(booking);

        result.PricePerPassenger.Should().Be(115m);
        result.TotalPrice.Should().Be(115m * booking.NumberOfPassengers);
        result.BookingReference.Should().StartWith("SR-");
        result.BookingDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task BookFlightAsync_SetsPassengerBookingId()
    {
        var flight = BuildFlight("f1", "GlobalAir", "US", "US", 100m);
        var booking = BuildBooking("f1");
        var strategy = Substitute.For<IFlightProviderStrategy>();
        strategy.CalculateFinalPrice(Arg.Any<decimal>()).Returns(100m);

        SetValidatorValid();
        _flightRepository.GetFlightByIdAsync("f1").Returns(flight);
        _flightRepository.GetAirportByIdAsync(Arg.Any<string>()).Returns((Airport?)null);
        _strategyFactory.GetStrategy(Arg.Any<string>()).Returns(strategy);
        _bookingRepository.CreateBookingAsync(Arg.Any<Booking>()).Returns(ci => ci.Arg<Booking>());

        var result = await _sut.BookFlightAsync(booking);

        result.Passengers.Should().AllSatisfy(p => p.BookingId.Should().Be(booking.Id));
    }

    [Fact]
    public async Task BookFlightAsync_SameCountryAirports_SetsIsInternationalFalse()
    {
        var flight = BuildFlight("f1", "GlobalAir", "US", "US", 100m);
        var booking = BuildBooking("f1");
        var strategy = Substitute.For<IFlightProviderStrategy>();
        strategy.CalculateFinalPrice(Arg.Any<decimal>()).Returns(100m);

        SetValidatorValid();
        _flightRepository.GetFlightByIdAsync("f1").Returns(flight);
        _strategyFactory.GetStrategy(Arg.Any<string>()).Returns(strategy);
        _bookingRepository.CreateBookingAsync(Arg.Any<Booking>()).Returns(ci => ci.Arg<Booking>());

        var result = await _sut.BookFlightAsync(booking);

        result.IsInternational.Should().BeFalse();
    }

    [Fact]
    public async Task BookFlightAsync_DifferentCountryAirports_SetsIsInternationalTrue()
    {
        var flight = BuildFlight("f1", "GlobalAir", "US", "UK", 100m);
        var booking = BuildBooking("f1");
        var strategy = Substitute.For<IFlightProviderStrategy>();
        strategy.CalculateFinalPrice(Arg.Any<decimal>()).Returns(100m);

        SetValidatorValid();
        _flightRepository.GetFlightByIdAsync("f1").Returns(flight);
        _strategyFactory.GetStrategy(Arg.Any<string>()).Returns(strategy);
        _bookingRepository.CreateBookingAsync(Arg.Any<Booking>()).Returns(ci => ci.Arg<Booking>());

        var result = await _sut.BookFlightAsync(booking);

        result.IsInternational.Should().BeTrue();
    }

    [Fact]
    public async Task BookFlightAsync_MissingAirportData_SetsIsInternationalFalse()
    {
        var flight = new Flight { Id = "f1", Provider = "GlobalAir", BaseFarePerPassenger = 100m, OriginAirportId = "a1", DestinationAirportId = "a2" };
        var booking = BuildBooking("f1");
        var strategy = Substitute.For<IFlightProviderStrategy>();
        strategy.CalculateFinalPrice(Arg.Any<decimal>()).Returns(100m);

        SetValidatorValid();
        _flightRepository.GetFlightByIdAsync("f1").Returns(flight);
        _flightRepository.GetAirportByIdAsync(Arg.Any<string>()).Returns((Airport?)null);
        _strategyFactory.GetStrategy(Arg.Any<string>()).Returns(strategy);
        _bookingRepository.CreateBookingAsync(Arg.Any<Booking>()).Returns(ci => ci.Arg<Booking>());

        var result = await _sut.BookFlightAsync(booking);

        result.IsInternational.Should().BeFalse();
    }

    [Fact]
    public async Task BookFlightAsync_InvalidBooking_ThrowsValidationException()
    {
        var booking = BuildBooking("f1");
        var failures = new[] { new FluentValidation.Results.ValidationFailure("FlightId", "Flight ID is required.") };
        _validator
            .ValidateAsync(booking, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(failures));

        var act = () => _sut.BookFlightAsync(booking);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task BookFlightAsync_FlightNotFound_ThrowsFlightNotFoundException()
    {
        var booking = BuildBooking("missing-flight");

        SetValidatorValid();
        _flightRepository.GetFlightByIdAsync("missing-flight").Returns((Flight?)null);

        var act = () => _sut.BookFlightAsync(booking);

        await act.Should().ThrowAsync<FlightNotFoundException>();
    }

    // --- GetBookingByReferenceAsync ---

    [Fact]
    public async Task GetBookingByReferenceAsync_ExistingReference_ReturnsBooking()
    {
        var expected = new Booking { BookingReference = "SR-ABC123" };
        _bookingRepository.GetBookingByReferenceAsync("SR-ABC123").Returns(expected);

        var result = await _sut.GetBookingByReferenceAsync("SR-ABC123");

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetBookingByReferenceAsync_NotFound_ThrowsBookingNotFoundException()
    {
        _bookingRepository.GetBookingByReferenceAsync("SR-ZZZZZZ").Returns((Booking?)null);

        var act = () => _sut.GetBookingByReferenceAsync("SR-ZZZZZZ");

        await act.Should().ThrowAsync<BookingNotFoundException>();
    }

    // --- Helpers ---

    private void SetValidatorValid() =>
        _validator
            .ValidateAsync(Arg.Any<Booking>(), Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());

    private static Flight BuildFlight(string id, string provider, string originCountry, string destinationCountry, decimal baseFare) =>
        new()
        {
            Id = id,
            Provider = provider,
            BaseFarePerPassenger = baseFare,
            OriginAirportId = "a1",
            DestinationAirportId = "a2",
            OriginAirport = new Airport { Id = "a1", Country = originCountry },
            DestinationAirport = new Airport { Id = "a2", Country = destinationCountry }
        };

    private static Booking BuildBooking(string flightId) =>
        new()
        {
            FlightId = flightId,
            NumberOfPassengers = 1,
            Passengers =
            [
                new Passenger
                {
                    FullName = "John Doe",
                    Email = "john@example.com",
                    DocumentType = "Passport Number",
                    DocumentNumber = "AB123456"
                }
            ]
        };
}
