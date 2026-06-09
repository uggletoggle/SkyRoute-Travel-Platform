using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SkyRouteTravel.Api.Dtos;
using SkyRouteTravel.Application.Bookings;
using SkyRouteTravel.Application.Flights;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapFlightEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/flights", async (
            [FromQuery] string? origin,
            [FromQuery] string? destination,
            [FromQuery] string? cabinClass,
            IFlightService flightService,
            IMapper mapper,
            [FromQuery] int limit = 10) =>
        {
            limit = Math.Clamp(limit, 1, 20);

            var flights = await flightService.SearchFlightsAsync(origin, destination, cabinClass);
            var flightDtos = mapper.Map<IEnumerable<FlightResponseDto>>(flights).Take(limit);
            var response = ApiResponse<FlightsResponseDto>.Success(
                new FlightsResponseDto { Flights = flightDtos },
                "Flights retrieved successfully.");
            return Results.Ok(response);
        })
        .WithName("GetFlights")
        .WithSummary("Search for flights")
        .WithDescription("Searches and filters available flights based on origin airport or city, destination airport or city, and cabin class. Maximum 20 results per request.");

        endpoints.MapGet("/airports", async (IFlightService flightService, IMapper mapper) =>
        {
            var airports = await flightService.GetAirportsAsync();
            var airportDtos = mapper.Map<IEnumerable<AirportResponseDto>>(airports);
            var response = ApiResponse<AirportsResponseDto>.Success(
                new AirportsResponseDto { Airports = airportDtos },
                "Airports retrieved successfully.");
            return Results.Ok(response);
        })
        .WithName("GetAirports")
        .WithSummary("Get list of airports")
        .WithDescription("Returns a list of all airports registered in the database, including their code, city, and country.");

        return endpoints;
    }

    public static IEndpointRouteBuilder MapProviderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/providers", async (IProviderService providerService) =>
        {
            var providers = await providerService.GetProvidersAsync();
            var response = ApiResponse<ProvidersResponseDto>.Success(
                new ProvidersResponseDto { Providers = providers },
                "Providers retrieved successfully.");
            return Results.Ok(response);
        })
        .WithName("GetProviders")
        .WithSummary("Get airline providers")
        .WithDescription("Returns a cached list of unique airline providers currently offering flights in the system.");

        return endpoints;
    }

    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/booking", async (
            [FromBody] BookFlightRequestDto request,
            IBookingService bookingService,
            IMapper mapper) =>
        {
            var bookingDomain = mapper.Map<Booking>(request);
            var createdBooking = await bookingService.BookFlightAsync(bookingDomain);
            var responseDto = mapper.Map<BookingResponseDto>(createdBooking);
            var response = ApiResponse<BookingResponseDto>.Success(
                responseDto,
                "Booking created successfully.");
            return Results.Created($"/booking/{responseDto.BookingReference}", response);
        })
        .WithName("BookFlight")
        .WithSummary("Create flight booking")
        .WithDescription("Creates a new flight booking for up to 9 passengers, applies contextual document validation rules, evaluates provider pricing strategies, and records details in the database.");

        endpoints.MapGet("/booking/{reference}", async (
            string reference,
            IBookingService bookingService,
            IMapper mapper) =>
        {
            var booking = await bookingService.GetBookingByReferenceAsync(reference);
            var responseDto = mapper.Map<BookingResponseDto>(booking);
            var response = ApiResponse<BookingResponseDto>.Success(
                responseDto,
                "Booking retrieved successfully.");
            return Results.Ok(response);
        })
        .WithName("GetBookingByReference")
        .WithSummary("Get booking by reference")
        .WithDescription("Retrieves a complete booking graph, including flight details and passenger lists, by its unique reference code (e.g. SR-XXXXXX).");

        return endpoints;
    }
}
