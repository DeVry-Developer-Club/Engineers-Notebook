﻿using System.ComponentModel.DataAnnotations;

namespace EngineerNotebook.Shared.Endpoints.Auth;

public class LoginRequest
{
    [Required, EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
}