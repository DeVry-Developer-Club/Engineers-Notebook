using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Tag;

namespace EngineerNotebook.Shared.Interfaces;

public interface ITagService
{
    Task<Tag> Create(CreateTagRequest request);
    Task<Tag> Update(UpdateTagRequest request);
    Task<DeleteResponse> Delete(string id);
    Task<Tag> GetById(string id);
    Task<List<Tag>> GetAll();
}