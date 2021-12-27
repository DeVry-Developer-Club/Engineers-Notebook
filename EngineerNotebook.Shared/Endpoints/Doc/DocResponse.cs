using System;

namespace EngineerNotebook.Shared.Endpoints.Doc;

public class DocResponse : IDtoResponse<DocDto>
{
    public DocDto Result { get; set; }
}

public class DocDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Contents { get; set; }
    public string CreatedByUserId { get; set; }
    public string EditedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset EditedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public string Id { get; set; }
}