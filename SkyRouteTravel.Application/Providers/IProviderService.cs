using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyRouteTravel.Application.Providers;

public interface IProviderService
{
    Task<IEnumerable<string>> GetProvidersAsync();
}
