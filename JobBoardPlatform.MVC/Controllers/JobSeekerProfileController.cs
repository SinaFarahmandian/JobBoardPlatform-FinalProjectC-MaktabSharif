using System.Security.Claims;
using JobBoardPlatform.Buisiness.Dtos.JobSeekerProfile;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "JobSeeker")]
public class JobSeekerProfileController : Controller
{
    private readonly IJobSeekerProfileService _service;
    public JobSeekerProfileController(IJobSeekerProfileService service) => _service = service;

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> Index() => View(await _service.GetMyProfileAsync(UserId));

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var profile = await _service.GetMyProfileAsync(UserId);
        return View(new UpdateJobSeekerProfileDto
        {
            FullName = profile.FullName, PhoneNumber = profile.PhoneNumber, Skills = profile.Skills,
            YearsOfExperience = profile.YearsOfExperience, DesiredJobTitle = profile.DesiredJobTitle
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateJobSeekerProfileDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _service.UpdateMyProfileAsync(UserId, dto);
        TempData["Success"] = "پروفایل به‌روزرسانی شد";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UploadResume(IFormFile file)
    {
        await _service.UploadResumeAsync(UserId, file);
        TempData["Success"] = "رزومه آپلود شد";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DownloadResume()
    {
        var (data, contentType, fileName) = await _service.GetResumeAsync(UserId);
        return File(data, contentType, fileName);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteResume()
    {
        await _service.DeleteResumeAsync(UserId);
        TempData["Success"] = "رزومه حذف شد";
        return RedirectToAction(nameof(Index));
    }
}