using System.ComponentModel.DataAnnotations;

namespace EngineerNotebook.Shared.Models.Requests
{
    public class LoginRequest
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}