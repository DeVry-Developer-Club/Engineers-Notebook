namespace EngineerNotebook.Shared.Authorization;
public class ClaimValue
{
    public string Type { get; set; }
    public string Value { get; set; }

    public ClaimValue()
    {
            
    }

    public ClaimValue(string type, string value)
    {
        Type = type;
        Value = value;
    }
}
