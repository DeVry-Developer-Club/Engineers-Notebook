namespace EngineerNotebook.Shared.Models.Requests
{
    public class UpdateTagRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TagType TagType { get; set; }
    }
}