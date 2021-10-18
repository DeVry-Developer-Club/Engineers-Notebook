using System.Security.Claims;
using System.Threading.Tasks;
using EngineerNotebook.Shared.Models.Requests;
using EngineerNotebook.Shared.Models.Responses;

namespace EngineerNotebook.Shared.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        ClaimsPrincipal ParseToken(string token);
    }
}