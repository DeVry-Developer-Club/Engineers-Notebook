using System.Threading.Tasks;

namespace EngineerNotebook.Core.Interfaces
{
    public interface ITokenClaimService
    {
        Task<string> GetTokenAsync(string userName);
    }
}