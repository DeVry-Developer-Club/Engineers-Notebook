namespace EngineerNotebook.Shared.Endpoints.Guide;

public class GetByTagsRequest
{
    public List<string> TagIds { get; set; } = new();
}