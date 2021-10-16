using System;
using System.Collections.Generic;
using EngineerNotebook.PublicApi.TagEndpoints;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    public class DocDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Contents { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset EditedAt { get; set; }
        
        public string CreatedByUserId { get; set; }
        public string EditedByUserId { get; set; }

        public string CreatedByUsername { get; set; }
        public string EditedByUsername { get; set; }

        public List<TagDto> Tags { get; set; } = new();
    }
}