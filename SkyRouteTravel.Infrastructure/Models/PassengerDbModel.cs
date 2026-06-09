using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyRouteTravel.Infrastructure.Models;

public class PassengerDbModel
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    [ForeignKey(nameof(BookingId))]
    public BookingDbModel? Booking { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
}
