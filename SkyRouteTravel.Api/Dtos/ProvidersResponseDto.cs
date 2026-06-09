namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Response DTO containing a collection of airline providers.
/// </summary>
public class ProvidersResponseDto
{
    /// <summary>
    /// The collection of provider names.
    /// </summary>
    public IEnumerable<string> Providers { get; set; } = [];
}
