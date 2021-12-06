using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Doc;

namespace EngineerNotebook.Shared.Interfaces;

public interface IDocService
{
    Task<Documentation> Create(CreateDocRequest request);
    Task<Documentation> Edit(UpdateDocRequest request);
    Task<DeleteResponse> Delete(string id);
    Task<Documentation> GetById(string id);
    Task<List<Documentation>> GetAll();
}