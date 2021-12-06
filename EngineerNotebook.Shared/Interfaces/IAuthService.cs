using System.Security.Claims;
using EngineerNotebook.Shared.Endpoints.Auth;

namespace EngineerNotebook.Shared.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> Login(LoginRequest request);
    ClaimsPrincipal ParseToken(string token);
}