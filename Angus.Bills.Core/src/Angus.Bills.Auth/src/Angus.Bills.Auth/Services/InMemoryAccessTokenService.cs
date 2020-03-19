using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Angus.Bills.Auth.Services
{
    internal sealed class InMemoryAccessTokenService : IAccessTokenService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtOptions _jwtOptions;

        public InMemoryAccessTokenService(IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor,
            JwtOptions jwtOptions)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> IsCurrentActiveToken()
        {
            return await IsActiveAsync(GetCurrentAsync());
        }

        public async Task DeactivateCurrentAsync()
        {
            await DeactivateAsync(GetCurrentAsync());
        }

        public Task<bool> IsActiveAsync(string token)
        {
            return Task.FromResult(string.IsNullOrWhiteSpace(_cache.Get<string>(GetKey(token))));
        }

        public Task DeactivateAsync(string token)
        {
            _cache.Set(GetKey(token), "revoked", new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(_jwtOptions.ExpiryMinutes)
            });

            return Task.CompletedTask;
        }

        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(' ').Last();
        }

        private static string GetKey(string token)
        {
            return $"blacklisted-tokens:{token}";
        }
    }
}