using Ardalis.Specification;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Specifications
{
    /// <summary>
    /// Filter for retrieving a record of <see cref="Documentation"/> with the <see cref="Tag"/>s associated to it
    /// </summary>
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