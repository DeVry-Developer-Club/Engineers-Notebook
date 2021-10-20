using System.Threading.Tasks;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Interfaces
{
    /// <summary>
    /// Service which is in charge of updating/creating/deleting records of <see cref="Tag"/>
    /// </summary>
    public interface ITagService
    {
        Task CreatedTagAsync(Tag tag);

        Task UpdateTagAsync(Tag tag);

        Task DeleteTagAsync(int id);
    }
}