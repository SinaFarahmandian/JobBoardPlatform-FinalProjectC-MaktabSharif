namespace JobBoardPlatform.Buisiness.Dtos.Auth;

using System.ComponentModel.DataAnnotations;

public class RegisterJobSeekerDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8, ErrorMessage = "The password must contain at least 8 characters")]
    public string Password { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }
}
