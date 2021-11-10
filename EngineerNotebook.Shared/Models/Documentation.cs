using System;
using System.Linq;

namespace EngineerNotebook.Shared.Models;
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

    public override string ToString()
    {
        return $"Title: {Title}\n" +
                $"Description: {Description}\n" +
                $"Contents: {Contents}\n" +
                $"Created By: {CreatedByUserId}\n" +
                $"Edited By: {EditedByUserId}\n" +
                $"Created At: {CreatedAt}\n" +
                $"Edited At: {EditedAt}\n" +
                $"Tags: {string.Join(", ", Tags.Select(x => x.Name))}\n";
    }
}
