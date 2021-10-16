using System.Threading.Tasks;
using Ardalis.GuardClauses;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Services
{
    public class TagService : ITagService
    {
        private readonly IAsyncRepository<Tag> _repository;

        public TagService(IAsyncRepository<Tag> repository)
        {
            _repository = repository;
        }

        public async Task CreatedTagAsync(Tag tag)
        {
            Guard.Against.Null(tag, nameof(tag));
            await _repository.AddAsync(tag);
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            Guard.Against.Null(tag, nameof(tag));
            await _repository.UpdateAsync(tag);
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(tag);
        }
    }
}