using Microsoft.Extensions.Caching.Memory;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Infrastructure.Repositories;

public class ProviderCachingRepositoryDecorator : IProviderRepository
{
    private readonly IProviderRepository _inner;
    private readonly IMemoryCache _cache;
    private const string ProvidersCacheKey = "ProvidersList";
    private static readonly TimeSpan CacheAbsoluteExpiration = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan CacheSlidingExpiration = TimeSpan.FromMinutes(2);

    public ProviderCachingRepositoryDecorator(IProviderRepository inner, IMemoryCache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<IEnumerable<string>> GetProvidersAsync()
    {
        if (!_cache.TryGetValue(ProvidersCacheKey, out IEnumerable<string>? providers) || providers == null)
        {
            providers = await _inner.GetProvidersAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheAbsoluteExpiration)
                .SetSlidingExpiration(CacheSlidingExpiration);

            _cache.Set(ProvidersCacheKey, providers, cacheEntryOptions);
        }

        return providers;
    }
}
