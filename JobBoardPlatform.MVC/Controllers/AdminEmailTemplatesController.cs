using JobBoardPlatform.Buisiness.Dtos.Email;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/email-templates")]
public class AdminEmailTemplatesController : Controller
{
    private readonly IAdminEmailTemplateService _service;
    public AdminEmailTemplatesController(IAdminEmailTemplateService service) => _service = service;

    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    [HttpGet("{key}/edit")]
    public async Task<IActionResult> Edit(string key)
    {
        var templates = await _service.GetAllAsync();
        var t = templates.First(x => x.Key == key);
        ViewData["Key"] = key;
        return View(new UpdateEmailTemplateDto { Subject = t.Subject, Body = t.Body, IsEnabled = t.IsEnabled });
    }

    [HttpPost("{key}/edit")]
    public async Task<IActionResult> Edit(string key, UpdateEmailTemplateDto dto)
    {
        await _service.UpdateAsync(key, dto);
        TempData["Success"] = "قالب ایمیل به‌روزرسانی شد";
        return RedirectToAction(nameof(Index));
    }
}