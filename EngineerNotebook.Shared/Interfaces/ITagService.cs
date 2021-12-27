using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Tag;

namespace EngineerNotebook.Shared.Interfaces;

public interface ITagService
{
    Task<TagDto?> Create(CreateTagRequest request);
    Task<TagDto> Update(UpdateTagRequest request);
    Task<DeleteResponse?> Delete(string id);
    Task<TagDto> GetById(string id);
    Task<List<TagDto>> GetAll();
}