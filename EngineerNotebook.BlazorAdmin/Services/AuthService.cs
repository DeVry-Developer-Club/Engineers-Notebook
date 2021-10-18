using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Authorization;
using EngineerNotebook.Shared.Interfaces;
using EngineerNotebook.Shared.Models.Requests;
using EngineerNotebook.Shared.Models.Responses;

namespace EngineerNotebook.BlazorAdmin.Services
{
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

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var response = await _httpService.HttpPost<LoginResponse>("authenticate", request);
            
            // Update cached token
            if (response.Result)
                await _storage.SetItemAsStringAsync(StorageConstants.IDENTITY, response.Token);

            return response;
        }

        public ClaimsPrincipal ParseToken(string tokenText)
        {
            JwtSecurityToken token = new JwtSecurityToken(tokenText);

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

            bool isAdmin = false;
            foreach (var claim in user.Claims)
            {
                if (claim.Type == "role" && claim.Value == "Administrators")
                    isAdmin = true;
                
                Console.WriteLine($"{claim.Type}: {claim.Value}");
                userIdentity.AddClaim(new Claim(claim.Type, claim.Value));
            }

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