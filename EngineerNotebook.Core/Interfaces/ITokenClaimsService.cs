using System.Threading.Tasks;

namespace EngineerNotebook.Core.Interfaces
{
    /// <summary>
    /// In charge of creating a token for users
    /// </summary>
    public interface ITokenClaimsService
    {
        Task<string> GetTokenAsync(string userName);
    }
}