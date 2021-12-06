namespace EngineerNotebook.Shared.Endpoints;

public class DeleteResponse
{
    public string Status { get; set; } = "Deleted";

    public int StatusCode { get; set; } = 200;
}