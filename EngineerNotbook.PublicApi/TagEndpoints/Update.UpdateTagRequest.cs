using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    public class UpdateTagRequest : BaseRequest
    {
        public string Name { get; set; }
        public TagType TagType { get; set; }
        public int Id { get; set; }
    }
}