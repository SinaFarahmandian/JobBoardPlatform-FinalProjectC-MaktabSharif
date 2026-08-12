using System.Security.Claims;
using JobBoardPlatform.Buisiness.Dtos.JobPosting;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "Employer")]
[Route("employer/job-postings")]
public class EmployerJobPostingsController : Controller
{
    private readonly IJobPostingService _service;
    public EmployerJobPostingsController(IJobPostingService service) => _service = service;

    private int EmployerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await _service.GetMyJobPostingsAsync(EmployerId));

    [HttpGet("create")]
    public IActionResult Create() => View(new CreateJobPostingDto());

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateJobPostingDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _service.CreateAsync(EmployerId, dto);
        TempData["Success"] = "The job posting has been created";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var jp = await _service.GetByIdAsync(EmployerId, id);
        ViewData["JobPostingId"] = id;
        return View(new UpdateJobPostingDto
        {
            Title = jp.Title, Description = jp.Description, Location = jp.Location,
            SalaryMin = jp.SalaryMin, SalaryMax = jp.SalaryMax, EmploymentType = jp.EmploymentType,
            ExpiresAt = jp.ExpiresAt, Category = jp.Category, Skills = jp.Skills
        });
    }

    [HttpPost("{id}/edit")]
    public async Task<IActionResult> Edit(int id, UpdateJobPostingDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewData["JobPostingId"] = id;
            return View(dto);
        }
        await _service.UpdateAsync(EmployerId, id, dto);
        TempData["Success"] = "The job posting has been updated";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(EmployerId, id);
        TempData["Success"] = "The job posting has been deleted";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        await _service.ToggleActiveAsync(EmployerId, id);
        return RedirectToAction(nameof(Index));
    }
}
