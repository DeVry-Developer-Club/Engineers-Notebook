using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using EngineerNotebook.Shared.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.BlazorAdmin
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        // Get default cache duration from config -- todo
        private static readonly TimeSpan UserCacheRefreshInterval = TimeSpan.FromSeconds(60);

        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomAuthStateProvider> _logger;

        private DateTimeOffset _userLastCheck = DateTimeOffset.FromUnixTimeSeconds(0);
        private ClaimsPrincipal _cachedUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(HttpClient httpClient,
            ILogger<CustomAuthStateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync() => new AuthenticationState(await GetUser(useCache: true));

        private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = false)
        {
            var now = DateTimeOffset.Now;

            if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
                return _cachedUser;

            _cachedUser = await FetchUser();
            _userLastCheck = now;

            return _cachedUser;
        }

        private async Task<ClaimsPrincipal> FetchUser()
        {
            UserInfo user = null;

            try
            {
                _logger.LogInformation("Fetching user details from web api.");
                user = await _httpClient.GetFromJsonAsync<UserInfo>("User");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Fetching user failed.");
            }

            if (user is not { IsAuthenticated: true })
                return null;

            var identity = new ClaimsIdentity(nameof(CustomAuthStateProvider),
                user.NameClaimType,
                user.RoleClaimType);
            
            if(user.Claims != null)
                foreach (var claim in user.Claims)
                    identity.AddClaim(new Claim(claim.Type, claim.Value));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            return new ClaimsPrincipal(identity);

        }
    }
}