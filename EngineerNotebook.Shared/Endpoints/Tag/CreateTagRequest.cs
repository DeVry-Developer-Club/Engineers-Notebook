using System.ComponentModel.DataAnnotations;

namespace EngineerNotebook.Shared.Endpoints.Tag;

public class CreateTagRequest : IDto
{
    [Required] public string Name { get; set; }
    public TagType TagType { get; set; }
}