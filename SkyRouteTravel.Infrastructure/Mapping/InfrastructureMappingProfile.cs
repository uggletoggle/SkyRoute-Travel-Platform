using AutoMapper;
using SkyRouteTravel.Application.Bookings;
using SkyRouteTravel.Application.Flights;
using SkyRouteTravel.Infrastructure.Models;

namespace SkyRouteTravel.Infrastructure.Mapping;

public class InfrastructureMappingProfile : Profile
{
    public InfrastructureMappingProfile()
    {
        CreateMap<AirportDbModel, Airport>().ReverseMap();
        CreateMap<FlightDbModel, Flight>().ReverseMap();
        CreateMap<BookingDbModel, Booking>().ReverseMap();
        CreateMap<PassengerDbModel, Passenger>().ReverseMap();
    }
}
