using System.Threading.Tasks;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Interfaces
{
    /// <summary>
    /// Service which is in charge of creating/updating/deleting <see cref="Documentation"/> records
    /// </summary>
    public interface IDocumentationService
    {
        Task CreateDocumentationAsync(Documentation doc);
        Task UpdateDocumentationAsync(Documentation doc, string editedByUserId);
        Task DeleteDocumentationAsync(int id);
    }
}