using System;

namespace EngineerNotebook.Shared.Models;
public class Documentation : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Contents { get; set; }

    public string CreatedByUserId { get; set; }
    public string EditedByUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EditedAt { get; set; } = DateTimeOffset.UtcNow;
        
    public List<string> Tags { get; set; } = new();
}
