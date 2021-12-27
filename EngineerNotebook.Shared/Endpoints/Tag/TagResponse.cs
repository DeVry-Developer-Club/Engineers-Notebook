namespace EngineerNotebook.Shared.Endpoints.Tag;

public class TagResponse : IDtoResponse<TagDto>
{
    public TagDto Result { get; set; }
}

public class TagDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public TagType TagType { get; set; } = TagType.Value;
}