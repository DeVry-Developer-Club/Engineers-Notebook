using System;
using System.Collections.Generic;
using EngineerNotebook.Shared.Interfaces;

namespace EngineerNotebook.Shared.Models
{
    public class Documentation : BaseEntity, IAggregateRoot
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Contents { get; set; }

        public string CreatedByUserId { get; set; }
        public string EditedByUserId { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset EditedAt { get; set; } = DateTimeOffset.UtcNow;
        
        public List<Tag> Tags { get; set; } = new();
    }
}