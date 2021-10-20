using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Data Object which represents a record of <see cref="Tag"/>
    /// </summary>
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TagType TagType { get; set; }
    }
}