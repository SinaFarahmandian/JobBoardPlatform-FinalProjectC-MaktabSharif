using System.Security.Claims;
using JobBoardPlatform.Buisiness.Dtos.JobApplication;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "JobSeeker")]
public class JobSeekerApplicationsController : Controller
{
    private readonly IJobSeekerApplicationService _service;
    public JobSeekerApplicationsController(IJobSeekerApplicationService service) => _service = service;

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> Index() => View(await _service.GetMyApplicationsAsync(UserId));

    public async Task<IActionResult> Details(int id) => View(await _service.GetDetailsAsync(UserId, id));

    [HttpPost]
    public async Task<IActionResult> Apply(CreateJobApplicationDto dto)
    {
        await _service.ApplyAsync(UserId, dto);
        TempData["Success"] = "درخواست شما ارسال شد";
        return RedirectToAction("Details", "Public", new { id = dto.JobPostingId });
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        await _service.CancelAsync(UserId, id);
        TempData["Success"] = "درخواست لغو شد";
        return RedirectToAction(nameof(Index));
    }
}