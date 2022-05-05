using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using nhsapp.sample.web.integration.TagHelpers;

namespace nhsapp.sample.web.integration.NhsLogin
{
    public interface INhsLoginSigningKeysProvider
    {
        Task<Option<JsonWebKeySet>> GetSigningKeys(string keyId);
    }
    public class NhsLoginSigningKeysProvider : INhsLoginSigningKeysProvider
    {
        private readonly INhsLoginClient _nhsLoginClient;
        private readonly ConcurrentDictionary<string, JsonWebKeySet> _keyCache;

        public NhsLoginSigningKeysProvider(INhsLoginClient nhsLoginClient)
        {
            _nhsLoginClient = nhsLoginClient;
            _keyCache = new ConcurrentDictionary<string, JsonWebKeySet>();
        }

        public async Task<Option<JsonWebKeySet>> GetSigningKeys(string keyId)
        {
            try
            {
                if (_keyCache.TryGetValue(keyId, out var existingCachedKeySet))
                {
                    return Option.Some(existingCachedKeySet);
                }

                if (!await UpdateKeyCache())
                {
                    return Option.None<JsonWebKeySet>();
                }

                if (_keyCache.TryGetValue(keyId, out var newCachedKeySet))
                {
                    return Option.Some(newCachedKeySet);
                }
                return Option.None<JsonWebKeySet>();
            }
            catch
            {
                return Option.None<JsonWebKeySet>();
            }
        }

        private async Task<bool> UpdateKeyCache()
        {
            var response = await _nhsLoginClient.GetSigningKeys();

            if (!response.HasSuccessStatusCode)
            {
                return false;
            }

            if (response.Body?.Keys is null)
            {
                return false;
            }

            foreach (var key in response.Body.Keys)
            {
                var cidKeyId = key?.Kid;

                if (cidKeyId != null)
                {
                    _keyCache[cidKeyId] = response.Body;
                }
            }

            return true;
        }
    }
}
