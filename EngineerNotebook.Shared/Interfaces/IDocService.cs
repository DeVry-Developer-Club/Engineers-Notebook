using System.Net;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Doc;

namespace EngineerNotebook.Shared.Interfaces;

public interface IDocService
{
    Task<DocDto?> Create(CreateDocRequest request);
    Task<(bool success, HttpStatusCode statusCode)> Edit(UpdateDocRequest request);
    Task<DeleteResponse> Delete(string id);
    Task<DocDto> GetById(string id);
    Task<List<DocDto>> GetAll();
}