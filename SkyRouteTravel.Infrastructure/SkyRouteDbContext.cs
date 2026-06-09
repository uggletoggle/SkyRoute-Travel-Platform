using Microsoft.EntityFrameworkCore;
using SkyRouteTravel.Infrastructure.Models;

namespace SkyRouteTravel.Infrastructure;

public class SkyRouteDbContext(DbContextOptions<SkyRouteDbContext> options) : DbContext(options)
{
    public DbSet<AirportDbModel> Airports => Set<AirportDbModel>();
    public DbSet<FlightDbModel> Flights => Set<FlightDbModel>();
    public DbSet<BookingDbModel> Bookings => Set<BookingDbModel>();
    public DbSet<PassengerDbModel> Passengers => Set<PassengerDbModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FlightDbModel>()
            .HasOne(f => f.OriginAirport)
            .WithMany()
            .HasForeignKey(f => f.OriginAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FlightDbModel>()
            .HasOne(f => f.DestinationAirport)
            .WithMany()
            .HasForeignKey(f => f.DestinationAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingDbModel>()
            .HasMany(b => b.Passengers)
            .WithOne(p => p.Booking)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static async Task SeedDataAsync(SkyRouteDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Airports.Any())
        {
            var jfk = new AirportDbModel { Id = Guid.NewGuid().ToString(), Code = "JFK", Name = "John F. Kennedy International Airport", City = "New York", Country = "USA" };
            var lhr = new AirportDbModel { Id = Guid.NewGuid().ToString(), Code = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" };
            var lax = new AirportDbModel { Id = Guid.NewGuid().ToString(), Code = "LAX", Name = "Los Angeles International Airport", City = "Los Angeles", Country = "USA" };
            var cdg = new AirportDbModel { Id = Guid.NewGuid().ToString(), Code = "CDG", Name = "Charles de Gaulle Airport", City = "Paris", Country = "France" };
            var hnd = new AirportDbModel { Id = Guid.NewGuid().ToString(), Code = "HND", Name = "Haneda Airport", City = "Tokyo", Country = "Japan" };

            context.Airports.AddRange(jfk, lhr, lax, cdg, hnd);
            await context.SaveChangesAsync();

            if (!context.Flights.Any())
            {
                context.Flights.AddRange(
                    new FlightDbModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Provider = "GlobalAir",
                        FlightNumber = "GA-101",
                        OriginAirportId = jfk.Id,
                        DestinationAirportId = lhr.Id,
                        DepartureTime = DateTime.UtcNow.AddDays(1),
                        ArrivalTime = DateTime.UtcNow.AddDays(1).AddHours(7),
                        DurationMinutes = 420,
                        CabinClass = "Economy",
                        BaseFarePerPassenger = 450.00m
                    },
                    new FlightDbModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Provider = "GlobalAir",
                        FlightNumber = "GA-102",
                        OriginAirportId = jfk.Id,
                        DestinationAirportId = lhr.Id,
                        DepartureTime = DateTime.UtcNow.AddDays(2),
                        ArrivalTime = DateTime.UtcNow.AddDays(2).AddHours(7),
                        DurationMinutes = 420,
                        CabinClass = "Business",
                        BaseFarePerPassenger = 1200.00m
                    },
                    new FlightDbModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Provider = "BudgetWings",
                        FlightNumber = "BW-201",
                        OriginAirportId = lax.Id,
                        DestinationAirportId = jfk.Id,
                        DepartureTime = DateTime.UtcNow.AddDays(1).AddHours(4),
                        ArrivalTime = DateTime.UtcNow.AddDays(1).AddHours(9).AddMinutes(30),
                        DurationMinutes = 330,
                        CabinClass = "Economy",
                        BaseFarePerPassenger = 150.00m
                    },
                    new FlightDbModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Provider = "BudgetWings",
                        FlightNumber = "BW-202",
                        OriginAirportId = lhr.Id,
                        DestinationAirportId = cdg.Id,
                        DepartureTime = DateTime.UtcNow.AddDays(3),
                        ArrivalTime = DateTime.UtcNow.AddDays(3).AddHours(1).AddMinutes(15),
                        DurationMinutes = 75,
                        CabinClass = "Economy",
                        BaseFarePerPassenger = 60.00m
                    },
                    new FlightDbModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Provider = "GlobalAir",
                        FlightNumber = "GA-501",
                        OriginAirportId = hnd.Id,
                        DestinationAirportId = lax.Id,
                        DepartureTime = DateTime.UtcNow.AddDays(5),
                        ArrivalTime = DateTime.UtcNow.AddDays(5).AddHours(10),
                        DurationMinutes = 600,
                        CabinClass = "First Class",
                        BaseFarePerPassenger = 3500.00m
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
