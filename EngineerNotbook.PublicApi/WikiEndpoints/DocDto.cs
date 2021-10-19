using System;
using System.Collections.Generic;
using EngineerNotebook.PublicApi.TagEndpoints;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Data Object which represents <see cref="Documentation"/>
    /// </summary>
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

        public List<TagDto> Tags { get; set; } = new();
    }
}