using System.Collections.Generic;
using System.Threading.Tasks;
using EngineerNotebook.Shared.Endpoints.Tag;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Shared.Interfaces
{
    public interface ITagService
    {
        Task<Tag> Create(CreateTagRequest request);
        Task<Tag> Edit(UpdateTagRequest request);
        Task<string> Delete(int id);
        Task<Tag> GetById(int id);
        Task<List<Tag>> GetAll();
    }
}