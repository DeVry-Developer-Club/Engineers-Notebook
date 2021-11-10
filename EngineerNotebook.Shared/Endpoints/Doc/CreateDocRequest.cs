using System.ComponentModel.DataAnnotations;

namespace EngineerNotebook.Shared.Endpoints.Doc;
public class CreateDocRequest : IDto
{
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Contents { get; set; }
    public List<int> TagIds { get; set; }
}
