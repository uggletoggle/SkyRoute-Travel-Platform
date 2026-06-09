namespace SkyRouteTravel.Application.Providers;

public interface IProviderRepository
{
    Task<IEnumerable<string>> GetProvidersAsync();
}
