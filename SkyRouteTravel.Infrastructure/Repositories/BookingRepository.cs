using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkyRouteTravel.Application.Bookings;
using SkyRouteTravel.Infrastructure.Models;

namespace SkyRouteTravel.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly SkyRouteDbContext _context;
    private readonly IMapper _mapper;

    public BookingRepository(SkyRouteDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        var dbBooking = _mapper.Map<BookingDbModel>(booking);

        _context.Bookings.Add(dbBooking);
        await _context.SaveChangesAsync();

        // Reload the booking with passengers and flight details populated
        var reloaded = await _context.Bookings
            .Include(b => b.Passengers)
            .Include(b => b.Flight)
                .ThenInclude(f => f!.OriginAirport)
            .Include(b => b.Flight)
                .ThenInclude(f => f!.DestinationAirport)
            .FirstOrDefaultAsync(b => b.Id == dbBooking.Id);

        return _mapper.Map<Booking>(reloaded ?? dbBooking);
    }

    public async Task<Booking?> GetBookingByReferenceAsync(string reference)
    {
        var dbBooking = await _context.Bookings
            .Include(b => b.Passengers)
            .Include(b => b.Flight)
                .ThenInclude(f => f!.OriginAirport)
            .Include(b => b.Flight)
                .ThenInclude(f => f!.DestinationAirport)
            .FirstOrDefaultAsync(b => b.BookingReference == reference);

        return _mapper.Map<Booking?>(dbBooking);
    }

    public async Task<bool> IsPassengerBookedOnFlightAsync(string flightId, string documentNumber)
    {
        return await _context.Bookings
            .Where(b => b.FlightId == flightId)
            .SelectMany(b => b.Passengers)
            .AnyAsync(p => p.DocumentNumber == documentNumber);
    }
}
