using System.Threading.Tasks;

namespace EngineerNotebook.Core.Interfaces
{
    public interface ITokenClaimsService
    {
        Task<string> GetTokenAsync(string userName);
    }
}