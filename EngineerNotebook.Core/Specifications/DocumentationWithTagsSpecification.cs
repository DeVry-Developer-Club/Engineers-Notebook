using Ardalis.Specification;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Specifications
{
    public sealed class DocumentationWithTagsSpecification : Specification<Documentation>
    {
        public DocumentationWithTagsSpecification(int docId)
        {
            Query
                .Where(x => x.Id == docId)
                .Include(x => x.Tags);
        }
    }
}