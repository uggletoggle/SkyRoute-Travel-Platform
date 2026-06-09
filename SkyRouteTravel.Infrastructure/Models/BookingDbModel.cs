using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyRouteTravel.Infrastructure.Models;

public class BookingDbModel
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string BookingReference { get; set; } = string.Empty;
    public string FlightId { get; set; } = string.Empty;
    [ForeignKey(nameof(FlightId))]
    public FlightDbModel? Flight { get; set; }
    public DateTime BookingDate { get; set; }
    public int NumberOfPassengers { get; set; }
    public decimal PricePerPassenger { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsInternational { get; set; }
    public List<PassengerDbModel> Passengers { get; set; } = new();
}
