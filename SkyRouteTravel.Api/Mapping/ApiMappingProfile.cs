using AutoMapper;
using SkyRouteTravel.Application.Bookings;
using SkyRouteTravel.Application.Flights;
using SkyRouteTravel.Application.Providers;
using SkyRouteTravel.Api.Dtos;

namespace SkyRouteTravel.Api.Mapping;

public class ApiMappingProfile : Profile
{
    private readonly IFlightProviderStrategyFactory _strategyFactory;

    public ApiMappingProfile(IFlightProviderStrategyFactory strategyFactory)
    {
        _strategyFactory = strategyFactory;

        CreateMap<Airport, AirportResponseDto>();

        CreateMap<Flight, FlightResponseDto>()
            .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => 
                _strategyFactory.GetStrategy(src.Provider).CalculateFinalPrice(src.BaseFarePerPassenger)));

        CreateMap<BookFlightRequestDto, Booking>()
            .ForMember(dest => dest.Passengers, opt => opt.MapFrom(src => src.Passengers));
        CreateMap<PassengerRequestDto, Passenger>();

        CreateMap<Booking, BookingResponseDto>();
        CreateMap<Passenger, PassengerResponseDto>();
    }
}
