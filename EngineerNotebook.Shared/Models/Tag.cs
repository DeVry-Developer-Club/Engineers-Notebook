using Newtonsoft.Json;

namespace EngineerNotebook.Shared.Models;
public enum TagType
{
    Prefix = 0,
    Value = 1,
    Phrase = 2
}
    
public class Tag : EntityBase
{
    public string Name { get; set; }
    public TagType TagType { get; set; } = TagType.Value;

    [JsonIgnore]
    public List<Documentation> Docs { get; set; }
}
