using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TagType TagType { get; set; }
    }
}