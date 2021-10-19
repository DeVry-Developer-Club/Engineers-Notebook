using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Request for creating a new record of <see cref="Tag"/>
    /// </summary>
    public class CreateTagRequest : BaseRequest
    {
        public string Name { get; set; }
        public TagType TagType { get; set; }
    }
}