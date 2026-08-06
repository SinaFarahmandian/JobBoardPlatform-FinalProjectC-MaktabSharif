using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.MVC.Models.Account;

public class LoginViewModel
{
    [Required, EmailAddress, Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Display(Name = "رمز عبور")]
    public string Password { get; set; } = string.Empty;
}