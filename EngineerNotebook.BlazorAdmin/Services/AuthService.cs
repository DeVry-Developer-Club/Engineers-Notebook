using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Authorization;
using EngineerNotebook.Shared.Endpoints.Auth;
using EngineerNotebook.Shared.Interfaces;

namespace EngineerNotebook.BlazorAdmin.Services
{
    /// <summary>
    /// Provides Authentication methods for our local user
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly HttpService _httpService;
        private readonly string _apiUrl;
        private readonly ILocalStorageService _storage;

        public AuthService(HttpService httpService, BaseUrlConfiguration config, ILocalStorageService storage)
        {
            _httpService = httpService;
            _storage = storage;
            _apiUrl = config.ApiBase;
        }

        /// <summary>
        /// Attempt to login with the given <paramref name="request"/>
        /// </summary>
        /// <param name="request">Contains username / password</param>
        /// <returns>Login Response</returns>
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var response = await _httpService.HttpPost<LoginResponse>("authenticate", request);
            
            // Update cached token
            if (response.Result)
                await _storage.SetItemAsStringAsync(StorageConstants.IDENTITY, response.Token);

            return response;
        }

        /// <summary>
        /// Used to parse local storage token so we can determine who our user is
        /// </summary>
        /// <param name="tokenText">Token</param>
        /// <returns>User identity based on <paramref name="tokenText"/></returns>
        public ClaimsPrincipal ParseToken(string tokenText)
        {
            JwtSecurityToken token = new JwtSecurityToken(tokenText);
            
            // Create our user information
            var user = new UserInfo
            {
                Token = tokenText,
                Claims = token.Claims.Select(x => new ClaimValue(x.Type, x.Value)),
                IsAuthenticated = true,
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };

            var userIdentity = new ClaimsIdentity(nameof(CustomAuthStateProvider),
                user.NameClaimType,
                user.RoleClaimType);

            // Our application requires the user to have administrative privileges (at least for now)
            bool isAdmin = false;
            foreach (var claim in user.Claims)
            {
                if (claim.Type == "role" && claim.Value == "Administrators")
                    isAdmin = true;
                
                Console.WriteLine($"{claim.Type}: {claim.Value}");
                userIdentity.AddClaim(new Claim(claim.Type, claim.Value));
            }

            // If they're not admin... error out
            if (!isAdmin)
            {
                Console.Error.WriteLine("User is not an administrator");
                return null;
            }
            
            return new ClaimsPrincipal(new ClaimsIdentity(nameof(CustomAuthStateProvider),
                user.NameClaimType,
                user.RoleClaimType));
        }

    }
}