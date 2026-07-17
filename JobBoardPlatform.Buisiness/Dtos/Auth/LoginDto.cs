namespace JobBoardPlatform.Buisiness.Dtos.Auth;
// LoginDto.cs
using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}