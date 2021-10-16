using System;
using System.Linq;
using Ardalis.Specification;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Specifications
{
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
        
        public DocumentationWithFiltersSpecification(string[] tagNames)
        {
            // Lowercase everything to make life easier
            tagNames = tagNames.Select(x => x.ToLower()).ToArray();
            
            Query
                .Include(x => x.Tags)
                .Where(x => 
                    ContainsEnough(x.Tags
                    .Select(y => y.Name.ToLower())
                    .Intersect(tagNames).Count(), tagNames.Length
            ));
        }

        public DocumentationWithFiltersSpecification(int[] tagIds)
        {
            Query.Include(x => x.Tags)
                .Where(x => 
                    ContainsEnough(x.Tags
                        .Select(y => y.Id)
                        .Intersect(tagIds).Count(), tagIds.Length
                ));
        }
    }
}