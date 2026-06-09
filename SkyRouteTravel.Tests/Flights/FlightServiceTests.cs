using FluentAssertions;
using NSubstitute;
using SkyRouteTravel.Application.Flights;

namespace SkyRouteTravel.Tests.Flights;

public class FlightServiceTests
{
    private readonly IFlightRepository _flightRepository;
    private readonly FlightService _sut;

    public FlightServiceTests()
    {
        _flightRepository = Substitute.For<IFlightRepository>();
        _sut = new FlightService(_flightRepository);
    }

    [Fact]
    public async Task SearchFlightsAsync_DelegatesToRepository()
    {
        var expected = new List<Flight> { new() { Id = "f1" } };
        _flightRepository
            .SearchFlightsAsync("JFK", "LAX", "Economy")
            .Returns(expected);

        var result = await _sut.SearchFlightsAsync("JFK", "LAX", "Economy");

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task SearchFlightsAsync_WithNullFilters_DelegatesToRepository()
    {
        var expected = new List<Flight> { new() { Id = "f1" }, new() { Id = "f2" } };
        _flightRepository
            .SearchFlightsAsync(null, null, null)
            .Returns(expected);

        var result = await _sut.SearchFlightsAsync(null, null, null);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task SearchFlightsAsync_WhenNoResultsFound_ReturnsEmptyCollection()
    {
        _flightRepository
            .SearchFlightsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(Enumerable.Empty<Flight>());

        var result = await _sut.SearchFlightsAsync("JFK", "LAX", "Business");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAirportsAsync_DelegatesToRepository()
    {
        var expected = new List<Airport>
        {
            new() { Id = "a1", Code = "JFK" },
            new() { Id = "a2", Code = "LAX" }
        };
        _flightRepository.GetAirportsAsync().Returns(expected);

        var result = await _sut.GetAirportsAsync();

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAirportsAsync_WhenNoAirports_ReturnsEmptyCollection()
    {
        _flightRepository.GetAirportsAsync().Returns(Enumerable.Empty<Airport>());

        var result = await _sut.GetAirportsAsync();

        result.Should().BeEmpty();
    }
}
