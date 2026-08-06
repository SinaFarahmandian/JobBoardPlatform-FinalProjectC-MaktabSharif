using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/jobseekers")]
public class AdminJobSeekersController : Controller
{
    private readonly IAdminJobSeekerService _service;
    public AdminJobSeekersController(IAdminJobSeekerService service) => _service = service;

    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id) => View(await _service.GetDetailsAsync(id));

    [HttpPost("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(int id, bool isActive)
    {
        await _service.SetActiveStatusAsync(id, isActive);
        TempData["Success"] = "وضعیت کارجو به‌روزرسانی شد";
        return RedirectToAction(nameof(Index));
    }
}