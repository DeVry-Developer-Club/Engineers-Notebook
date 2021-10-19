using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    public class CreateTagRequest : BaseRequest
    {
        public string Name { get; set; }
        public TagType TagType { get; set; }
    }
}