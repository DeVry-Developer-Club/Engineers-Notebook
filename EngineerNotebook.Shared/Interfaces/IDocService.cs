using EngineerNotebook.Shared.Endpoints.Doc;

namespace EngineerNotebook.Shared.Interfaces;
public interface IDocService
{
    Task<Documentation> Create(CreateDocRequest request);
    Task<Documentation> Edit(UpdateDocRequest request);
    Task<string> Delete(int id);
    Task<Documentation> GetById(int id);
    Task<List<Documentation>> GetAll();
}
