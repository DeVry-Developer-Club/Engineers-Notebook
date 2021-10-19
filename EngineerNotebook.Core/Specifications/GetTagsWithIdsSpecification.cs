using System.Linq;
using Ardalis.Specification;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Specifications
{
    public class GetTagsWithIdsSpecification : Specification<Tag>
    {
        public GetTagsWithIdsSpecification(int[] ids)
        {
            Query
                .Where(x => ids.Contains(x.Id));
        }
    }
}