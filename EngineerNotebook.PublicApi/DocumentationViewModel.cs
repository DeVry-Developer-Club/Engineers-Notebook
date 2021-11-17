namespace EngineerNotebook.PublicApi;
internal class DocumentationViewModel
{
    public Shared.Models.Documentation Doc { get; set; }
    public List<Shared.Models.Tag> Tags { get; set; } = new();
}
