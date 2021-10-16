using System.Threading.Tasks;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Interfaces
{
    public interface IDocumentationService
    {
        Task CreateDocumentationAsync(Documentation doc);
        Task UpdateDocumentationAsync(Documentation doc, string editedByUserId);
        Task DeleteDocumentationAsync(int id);
    }
}