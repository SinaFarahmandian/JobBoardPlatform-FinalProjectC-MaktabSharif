namespace JobBoardPlatform.Buisiness.Dtos.Auth;
// RegisterEmployerDto.cs
using System.ComponentModel.DataAnnotations;

public class RegisterEmployerDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    [Required, StringLength(150, MinimumLength = 2)]
    public string CompanyName { get; set; } = string.Empty;

    [Url]
    public string? CompanyWebsite { get; set; }

    public string? CompanyDescription { get; set; }
    public string? Industry { get; set; }
}