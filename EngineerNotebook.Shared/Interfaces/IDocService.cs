using System.Collections.Generic;
using System.Threading.Tasks;
using EngineerNotebook.Shared.Models;
using EngineerNotebook.Shared.Models.Requests;

namespace EngineerNotebook.Shared.Interfaces
{
    public interface IDocService
    {
        Task<Documentation> Create(CreateDocRequest request);
        Task<Documentation> Edit(UpdateDocRequest request);
        Task<string> Delete(int id);
        Task<Documentation> GetById(int id);
        Task<List<Documentation>> GetAll();
    }
}