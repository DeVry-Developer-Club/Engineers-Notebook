using System;
using System.Linq;
using Ardalis.Specification;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Specifications
{
    /// <summary>
    /// Query for retrieving all records of <see cref="Documentation"/> that match the provided criteria
    /// </summary>
    public sealed class DocumentationWithFiltersSpecification : Specification<Documentation>
    {
        private const float NeedAtLeast = 0.65f; // Need at least X of the expected tags
        private const int ThresholdCheck = 3; // Start checking this once we have expected 3+
        
        /*
            So if we have 3 values
                3 * .65 = 1.95
                    -> rounded up = 2
                        -> must have at least 2 tags
                        
            So if we have 7 values
                7 * .65 = 4.55
                    -> rounded up = 5
                        -> must have at least 5 tags
         */
        
        bool ContainsEnough(int intersectionCount, int expected)
        {
            if (expected < ThresholdCheck)
                return true;

            int mustHave = (int) Math.Ceiling(expected * NeedAtLeast);
            return intersectionCount >= mustHave;
        }

        public DocumentationWithFiltersSpecification(int[] tagIds)
        {
            int required = (int)Math.Ceiling(tagIds.Length * NeedAtLeast);
            
            Query.Include(x => x.Tags)// we need the tags to be part of the result set
                .Where(x=> x.Tags
                        .Select(y=>y.Id)
                        .Count(z => tagIds.Contains(z)) >= required
                );
        }
    }
}