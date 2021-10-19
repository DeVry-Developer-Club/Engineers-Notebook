using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Request for updating a <see cref="Tag"/>
    /// </summary>
    public class UpdateTagRequest : BaseRequest
    {
        public string Name { get; set; }
        public TagType TagType { get; set; }
        public int Id { get; set; }
    }
}