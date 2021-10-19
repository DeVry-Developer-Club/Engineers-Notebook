using Ardalis.Specification;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Specifications
{
    /// <summary>
    /// Query for retrieving all records of <see cref="Documentation"/>
    /// </summary>
    public sealed class DocumentationListAllSpecification : Specification<Documentation>
    {
        public DocumentationListAllSpecification()
        {
            Query.Include(x => x.Tags);
        }
    }
}