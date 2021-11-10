using Newtonsoft.Json;

namespace EngineerNotebook.Shared.Models;
public enum TagType
{
    Prefix,
    Value,
    Phrase
}
    
public class Tag : BaseEntity, IAggregateRoot
{
    public string Name { get; set; }
    public TagType TagType { get; set; } = TagType.Value;

    [JsonIgnore]
    public List<Documentation> Docs { get; set; }
}
