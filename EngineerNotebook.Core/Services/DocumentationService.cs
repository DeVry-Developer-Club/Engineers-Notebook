using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Core.Specifications;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Services
{
    public class DocumentationService : IDocumentationService
    {
        private readonly IAsyncRepository<Documentation> _repository;

        public DocumentationService(IAsyncRepository<Documentation> repository)
        {
            _repository = repository;
        }

        public async Task CreateDocumentationAsync(Documentation doc)
        {
            Guard.Against.Null(doc, nameof(doc));
            await _repository.AddAsync(doc);
        }

        public async Task UpdateDocumentationAsync(Documentation doc, string editedByUserId)
        {
            Guard.Against.Null(doc, nameof(doc));
            Guard.Against.NullOrEmpty(editedByUserId, nameof(editedByUserId));
            
            doc.EditedByUserId = editedByUserId;
            doc.EditedAt = DateTimeOffset.UtcNow;
            
            await _repository.UpdateAsync(doc);
        }

        public async Task DeleteDocumentationAsync(int id)
        {
            var doc = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(doc);
        }
    }
}