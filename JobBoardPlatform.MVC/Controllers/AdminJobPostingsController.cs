using JobBoardPlatform.Buisiness.Dtos.Admin;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/job-postings")]
public class AdminJobPostingsController : Controller
{
    private readonly IAdminJobPostingService _service;
    public AdminJobPostingsController(IAdminJobPostingService service) => _service = service;

    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    [HttpPost("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(int id, bool isActive)
    {
        await _service.SetActiveStatusAsync(id, isActive);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        TempData["Success"] = "The job posting has been deleted";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id}/featured")]
    public async Task<IActionResult> SetFeatured(int id, bool isFeatured, DateTime? featuredUntil)
    {
        await _service.SetFeaturedAsync(id, new SetFeaturedDto { IsFeatured = isFeatured, FeaturedUntil = featuredUntil });
        return RedirectToAction(nameof(Index));
    }
}
