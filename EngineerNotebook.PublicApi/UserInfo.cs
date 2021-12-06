namespace EngineerNotebook.PublicApi;

public class UserInfo
{
    public static readonly UserInfo Anonymous = new();
    public bool IsAuthenticated { get; set; }
    public string NameClaimType { get; set; }
    public string RoleClaimType { get; set; }
    public IEnumerable<ClaimValue> Claims { get; set; }
    public string Token { get; set; }
}

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