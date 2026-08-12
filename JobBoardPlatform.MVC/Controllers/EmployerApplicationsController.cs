using System.Security.Claims;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "Employer")]
[Route("employer/applications")]
public class EmployerApplicationsController : Controller
{
    private readonly IEmployerApplicationService _service;
    public EmployerApplicationsController(IEmployerApplicationService service) => _service = service;

    private int EmployerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("job-posting/{jobPostingId}")]
    public async Task<IActionResult> Index(int jobPostingId)
    {
        ViewData["JobPostingId"] = jobPostingId;
        return View(await _service.GetApplicationsForJobPostingAsync(EmployerId, jobPostingId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id) => View(await _service.GetApplicationDetailsAsync(EmployerId, id));

    [HttpPost("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, ApplicationStatus newStatus, int jobPostingId)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The selected status is not valid";
            return RedirectToAction(nameof(Index), new { jobPostingId });
        }

        await _service.ChangeStatusAsync(EmployerId, id, newStatus);
        TempData["Success"] = "The application status has been updated";
        return RedirectToAction(nameof(Index), new { jobPostingId });
    }
}
