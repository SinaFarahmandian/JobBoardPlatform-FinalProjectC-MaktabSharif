using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.MVC.Models.Account;

public class RegisterEmployerViewModel
{
    [Required, StringLength(100, MinimumLength = 2), Display(Name = "نام کامل")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8), DataType(DataType.Password), Display(Name = "رمز عبور")]
    public string Password { get; set; } = string.Empty;

    [Required, StringLength(150, MinimumLength = 2), Display(Name = "نام شرکت")]
    public string CompanyName { get; set; } = string.Empty;

    [Url, Display(Name = "وب‌سایت شرکت")]
    public string? CompanyWebsite { get; set; }
}