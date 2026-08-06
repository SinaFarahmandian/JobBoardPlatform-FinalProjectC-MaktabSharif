using JobBoardPlatform.Buisiness.Dtos.Auth;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities;
using JobBoardPlatform.Domain.Entities.Employers;
using JobBoardPlatform.MVC.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(IAuthService authService, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _authService = authService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult RegisterJobSeeker() => View(new RegisterJobSeekerViewModel());

    [HttpPost]
    public async Task<IActionResult> RegisterJobSeeker(RegisterJobSeekerViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _authService.RegisterJobSeekerAsync(new RegisterJobSeekerDto
        {
            FullName = model.FullName, Email = model.Email, Password = model.Password
        });

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        TempData["Success"] = "ثبت‌نام با موفقیت انجام شد. اکنون وارد شوید.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult RegisterEmployer() => View(new RegisterEmployerViewModel());

    [HttpPost]
    public async Task<IActionResult> RegisterEmployer(RegisterEmployerViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _authService.RegisterEmployerAsync(new RegisterEmployerDto
        {
            FullName = model.FullName, Email = model.Email, Password = model.Password,
            CompanyName = model.CompanyName, CompanyWebsite = model.CompanyWebsite
        });

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        TempData["Success"] = "ثبت‌نام با موفقیت انجام شد. حساب شما تا تأیید ادمین غیرفعال است.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            ModelState.AddModelError(string.Empty, "ایمیل یا رمز عبور اشتباه است");
            return View(model);
        }

        if (!user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "حساب شما غیرفعال شده است");
            return View(model);
        }

        if (user is Employer && !user.IsApproved)
        {
            ModelState.AddModelError(string.Empty, "حساب کارفرمایی شما هنوز توسط ادمین تأیید نشده است");
            return View(model);
        }

        await _signInManager.SignInAsync(user, isPersistent: true);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Public");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Public");
    }

    public IActionResult AccessDenied() => View();
}