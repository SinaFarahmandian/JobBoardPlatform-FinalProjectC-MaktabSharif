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

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        await _service.SetActiveStatusAsync(id, true);
        TempData["Success"] = "The job seeker account has been activated";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        await _service.SetActiveStatusAsync(id, false);
        TempData["Success"] = "The job seeker account has been deactivated";
        return RedirectToAction(nameof(Index));
    }
}
