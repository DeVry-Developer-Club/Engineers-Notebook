namespace EngineerNotebook.Shared.Endpoints.Doc;

public class DocResponse : IDtoResponse<Documentation>
{
    public Documentation Result { get; set; }
}