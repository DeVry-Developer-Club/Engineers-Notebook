using System.Threading.Tasks;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Interfaces
{
    public interface ITagService
    {
        Task CreatedTagAsync(Tag tag);

        Task UpdateTagAsync(Tag tag);

        Task DeleteTagAsync(int id);
    }
}