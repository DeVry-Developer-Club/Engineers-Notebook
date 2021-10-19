using System.ComponentModel.DataAnnotations;

namespace EngineerNotebook.Shared.Endpoints.Auth
{
    public class LoginRequest
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}