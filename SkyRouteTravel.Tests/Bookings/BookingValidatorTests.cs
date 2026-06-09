using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using SkyRouteTravel.Application.Bookings;
using SkyRouteTravel.Application.Flights;

namespace SkyRouteTravel.Tests.Bookings;

public class BookingValidatorTests
{
    private readonly IFlightRepository _flightRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly BookingValidator _sut;

    public BookingValidatorTests()
    {
        _flightRepository = Substitute.For<IFlightRepository>();
        _bookingRepository = Substitute.For<IBookingRepository>();
        _sut = new BookingValidator(_flightRepository, _bookingRepository);

        // Safe defaults: flight exists, passenger not yet booked
        _flightRepository
            .GetFlightByIdAsync(Arg.Any<string>())
            .Returns(new Flight { Id = "f1" });
        _bookingRepository
            .IsPassengerBookedOnFlightAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(false);
    }

    // --- FlightId ---

    [Fact]
    public async Task FlightId_WhenEmpty_ShouldHaveValidationError()
    {
        var booking = ValidBooking();
        booking.FlightId = string.Empty;

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldHaveValidationErrorFor(b => b.FlightId)
            .WithErrorMessage("Flight ID is required.");
    }

    [Fact]
    public async Task FlightId_WhenFlightDoesNotExist_ShouldHaveValidationError()
    {
        _flightRepository.GetFlightByIdAsync("bad-id").Returns((Flight?)null);
        var booking = ValidBooking();
        booking.FlightId = "bad-id";

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldHaveValidationErrorFor(b => b.FlightId)
            .WithErrorMessage("The specified flight does not exist.");
    }

    [Fact]
    public async Task FlightId_WhenFlightExists_ShouldNotHaveValidationError()
    {
        var booking = ValidBooking();

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldNotHaveValidationErrorFor(b => b.FlightId);
    }

    // --- NumberOfPassengers ---

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(-1)]
    public async Task NumberOfPassengers_OutOfRange_ShouldHaveValidationError(int count)
    {
        var booking = ValidBooking();
        booking.NumberOfPassengers = count;
        booking.Passengers = BuildPassengers(Math.Max(count, 1));

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldHaveValidationErrorFor(b => b.NumberOfPassengers)
            .WithErrorMessage("Number of passengers must be between 1 and 9.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(9)]
    public async Task NumberOfPassengers_InRange_ShouldNotHaveValidationError(int count)
    {
        var booking = ValidBooking(passengerCount: count);

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldNotHaveValidationErrorFor(b => b.NumberOfPassengers);
    }

    // --- Passengers list ---

    [Fact]
    public async Task Passengers_WhenEmpty_ShouldHaveValidationError()
    {
        var booking = ValidBooking();
        booking.Passengers = [];

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldHaveValidationErrorFor(b => b.Passengers)
            .WithErrorMessage("Passenger list cannot be empty.");
    }

    [Fact]
    public async Task Passengers_WhenCountDoesNotMatchNumberOfPassengers_ShouldHaveValidationError()
    {
        var booking = ValidBooking(passengerCount: 1);
        booking.NumberOfPassengers = 2;

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldHaveValidationErrorFor(b => b.Passengers);
    }

    [Fact]
    public async Task Passengers_WithDuplicateDocumentNumbers_ShouldHaveValidationError()
    {
        var booking = ValidBooking(passengerCount: 2);
        booking.Passengers[0].DocumentNumber = "AB123456";
        booking.Passengers[1].DocumentNumber = "AB123456";

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldHaveValidationErrorFor(b => b.Passengers)
            .WithErrorMessage("Duplicate identification numbers found. Each passenger must have a unique identification number.");
    }

    [Fact]
    public async Task Passengers_WithDuplicateDocumentNumbersCaseInsensitive_ShouldHaveValidationError()
    {
        var booking = ValidBooking(passengerCount: 2);
        booking.Passengers[0].DocumentNumber = "AB123456";
        booking.Passengers[1].DocumentNumber = "ab123456";

        var result = await _sut.TestValidateAsync(booking);

        result.ShouldHaveValidationErrorFor(b => b.Passengers)
            .WithErrorMessage("Duplicate identification numbers found. Each passenger must have a unique identification number.");
    }

    // --- Passenger already booked ---

    [Fact]
    public async Task Passengers_WhenPassengerAlreadyBookedFlight_ShouldHaveValidationError()
    {
        _bookingRepository
            .IsPassengerBookedOnFlightAsync("f1", "AB123451")
            .Returns(true);

        var booking = ValidBooking();

        var result = await _sut.TestValidateAsync(booking);

        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("AB123451") &&
            e.ErrorMessage.Contains("already booked this flight"));
    }

    [Fact]
    public async Task Passengers_WhenPassengerNotPreviouslyBooked_ShouldNotHaveError()
    {
        var booking = ValidBooking();

        var result = await _sut.TestValidateAsync(booking);

        result.Errors.Should().NotContain(e => e.ErrorMessage.Contains("already booked this flight"));
    }

    // --- Helpers ---

    private static Booking ValidBooking(int passengerCount = 1) =>
        new()
        {
            FlightId = "f1",
            NumberOfPassengers = passengerCount,
            Passengers = BuildPassengers(passengerCount)
        };

    private static List<Passenger> BuildPassengers(int count) =>
        Enumerable.Range(1, count).Select(i => new Passenger
        {
            FullName = $"Passenger {i}",
            Email = $"passenger{i}@example.com",
            DocumentType = "Passport Number",
            DocumentNumber = $"AB12345{i}"
        }).ToList();
}
