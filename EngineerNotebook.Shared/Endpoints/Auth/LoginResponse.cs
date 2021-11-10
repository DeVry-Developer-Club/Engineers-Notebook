namespace EngineerNotebook.Shared.Endpoints.Auth;
public class LoginResponse
{
    public bool Result { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsLockedOut { get; set; }
    public bool IsNotAllowed { get; set; }
    public bool RequiresTwoFactor { get; set; }
}
