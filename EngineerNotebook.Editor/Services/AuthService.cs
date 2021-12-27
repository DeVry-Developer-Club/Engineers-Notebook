using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using EngineerNotebook.Shared.Authorization;
using EngineerNotebook.Shared.Endpoints.Auth;
using EngineerNotebook.Shared.Interfaces;

namespace EngineerNotebook.Editor.Services;

/// <summary>
/// Provides authentication methods for local user
/// </summary>
public class AuthService : IAuthService
{
    private readonly HttpService _httpService;
    private readonly ILocalStorageService _storage;

    public AuthService(HttpService service, ILocalStorageService storage)
    {
        _httpService = service;
        _storage = storage;
    }

    /// <summary>
    /// Attempt to login with the given <paramref name="request"/>
    /// </summary>
    /// <param name="request">Contains username/password</param>
    /// <returns>Login response</returns>
    public async Task<LoginResponse?> Login(LoginRequest request)
    {
        var response = await _httpService.HttpPost<LoginResponse>("auth", request);

        if (response is null)
            return null;
        
        // update cached token
        if (response.Success)
            await _storage.SetItemAsStringAsync(StorageConstants.IDENTITY, response.Token);

        return response;
    }

    /// <summary>
    /// Used to parsed local storage token so we can determine who our user is
    /// </summary>
    /// <param name="tokenText">Token</param>
    /// <returns>User identity based on <paramref name="tokenText"/></returns>
    public ClaimsPrincipal ParseToken(string tokenText)
    {
        JwtSecurityToken token = new JwtSecurityToken(tokenText);
        
        // create our user information
        var user = new UserInfo
        {
            Token = tokenText,
            Claims = token.Claims.Select(x => new ClaimValue(x.Type, x.Value)),
            IsAuthenticated = true,
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name
        };

        var userIdentity = new ClaimsIdentity( "Bearer",
            user.NameClaimType,
            user.RoleClaimType);
        
        foreach (var claim in user.Claims)
            userIdentity.AddClaim(new(claim.Type, claim.Value));

        return new ClaimsPrincipal(userIdentity);
    }
}