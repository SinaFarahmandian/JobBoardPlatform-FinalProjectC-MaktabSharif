namespace JobBoardPlatform.Buisiness.Dtos.Auth;

// RegisterJobSeekerDto.cs
using System.ComponentModel.DataAnnotations;

public class RegisterJobSeekerDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8, ErrorMessage = "رمز عبور باید حداقل ۸ کاراکتر باشد")]
    public string Password { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }
}