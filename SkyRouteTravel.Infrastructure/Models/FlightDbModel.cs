using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyRouteTravel.Infrastructure.Models;

public class FlightDbModel
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    
    public string OriginAirportId { get; set; } = string.Empty;
    [ForeignKey(nameof(OriginAirportId))]
    public AirportDbModel? OriginAirport { get; set; }
    
    public string DestinationAirportId { get; set; } = string.Empty;
    [ForeignKey(nameof(DestinationAirportId))]
    public AirportDbModel? DestinationAirport { get; set; }
    
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int DurationMinutes { get; set; }
    public string CabinClass { get; set; } = string.Empty;
    public decimal BaseFarePerPassenger { get; set; }
}
