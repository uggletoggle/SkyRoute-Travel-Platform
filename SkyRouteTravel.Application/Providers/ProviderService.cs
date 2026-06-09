using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyRouteTravel.Application.Providers;

public class ProviderService : IProviderService
{
    private readonly IProviderRepository _providerRepository;

    public ProviderService(IProviderRepository providerRepository)
    {
        _providerRepository = providerRepository;
    }

    public Task<IEnumerable<string>> GetProvidersAsync()
    {
        return _providerRepository.GetProvidersAsync();
    }
}
