using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using EngineerNotebook.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace EngineerNotebook.Editor;

public class EditorAuthStateProvider : AuthenticationStateProvider
{
    private static readonly TimeSpan UserCacheRefreshInterval = TimeSpan.FromSeconds(60);

    private readonly HttpClient _httpClient;

    private DateTimeOffset _userLastCheck = DateTimeOffset.FromUnixTimeSeconds(0);
    private ClaimsPrincipal _cachedUser = new(new ClaimsIdentity());
    private readonly ILocalStorageService _storage;
    private readonly IAuthService _authService;

    public EditorAuthStateProvider(HttpClient httpClient,
        ILocalStorageService storage,
        IAuthService authService)
    {
        _httpClient = httpClient;
        _storage = storage;
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() =>
        new(await GetUser(useCache: true) ?? new());

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
        if (_cachedUser is {Identity.IsAuthenticated: true})
            return _cachedUser;

        if (!await _storage.ContainKeyAsync(StorageConstants.IDENTITY))
            return null;

        string result = await _storage.GetItemAsStringAsync(StorageConstants.IDENTITY);
        await _storage.SetItemAsStringAsync(StorageConstants.IDENTITY, result);

        _cachedUser = _authService.ParseToken(result);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result);
        return _cachedUser;
    }
}