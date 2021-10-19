namespace EngineerNotebook.Shared.Models.Requests
{
    public class CreateTagRequest
    {
        public string Name { get; set; }
        public TagType TagType { get; set; }
    }
}