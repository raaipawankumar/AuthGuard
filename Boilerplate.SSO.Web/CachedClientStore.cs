using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Memory;

namespace Boilerplate.SSO.Web
{
    public class CachedClientStore : IClientStore
    {
        private readonly IClientStore _inner;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public CachedClientStore(IClientStore inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
        }

        public async Task<Client?> FindClientByIdAsync(string clientId)
        {
            if (_cache.TryGetValue(clientId, out Client client))
            {
                return client;
            }

            client = await _inner.FindClientByIdAsync(clientId);
            if (client != null)
            {
                _cache.Set(clientId, client, _cacheOptions);
            }

            return client;
        }
    }
}