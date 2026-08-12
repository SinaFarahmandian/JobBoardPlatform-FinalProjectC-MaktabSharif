using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.MVC.Models.Account;

public class RegisterJobSeekerViewModel
{
    [Required, StringLength(100, MinimumLength = 2), Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "Email address")]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8), DataType(DataType.Password), Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}
