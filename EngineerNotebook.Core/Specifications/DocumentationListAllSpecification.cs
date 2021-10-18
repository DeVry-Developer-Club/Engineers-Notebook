using Ardalis.Specification;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Specifications
{
    public sealed class DocumentationListAllSpecification : Specification<Documentation>
    {
        public DocumentationListAllSpecification()
        {
            Query.Include(x => x.Tags);
        }
    }
}