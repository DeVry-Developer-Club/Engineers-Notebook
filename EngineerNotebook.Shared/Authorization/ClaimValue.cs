namespace EngineerNotebook.Shared.Authorization;

public struct ClaimValue
{
    public string Type { get; }
    public string Value { get; }

    public ClaimValue(string type, string value)
    {
        Type = type;
        Value = value;
    }
}