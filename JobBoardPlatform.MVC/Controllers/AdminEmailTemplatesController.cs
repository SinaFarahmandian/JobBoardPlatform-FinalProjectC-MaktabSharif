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
        var t = templates.FirstOrDefault(x => x.Key == key);
        if (t == null) return NotFound();
        ViewData["Key"] = key;
        return View(new UpdateEmailTemplateDto { Subject = t.Subject, Body = t.Body, IsEnabled = t.IsEnabled });
    }

    [HttpPost("{key}/edit")]
    public async Task<IActionResult> Edit(string key, UpdateEmailTemplateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Key"] = key;
            return View(dto);
        }

        await _service.UpdateAsync(key, dto);
        TempData["Success"] = "The email template has been updated";
        return RedirectToAction(nameof(Index));
    }
}
