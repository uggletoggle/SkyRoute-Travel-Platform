using System.ComponentModel.DataAnnotations;

namespace SkyRouteTravel.Infrastructure.Models;

public class AirportDbModel
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
