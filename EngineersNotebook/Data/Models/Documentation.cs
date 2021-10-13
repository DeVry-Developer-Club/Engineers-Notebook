using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EngineersNotebook.Data.Models
{
    public class Documentation
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }
        public IdentityUser CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime EditedAt { get; set; } = DateTime.UtcNow;
        
        public string EditedByUserId { get; set; }
        public IdentityUser EditedByUser { get; set; }

        [Required]
        public string Contents { get; set; }

        public List<Tag> Tags { get; set; } = new();
    }
}