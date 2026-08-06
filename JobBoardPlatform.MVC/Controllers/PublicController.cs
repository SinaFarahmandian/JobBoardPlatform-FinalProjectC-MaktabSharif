using JobBoardPlatform.Buisiness.Dtos.JobPosting;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

public class PublicController : Controller
{
    private readonly IPublicJobPostingService _service;
    public PublicController(IPublicJobPostingService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] JobPostingSearchQueryDto query)
    {
        var result = await _service.SearchAsync(query);
        ViewData["Query"] = query;
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var posting = await _service.GetByIdAsync(id);
        return View(posting);
    }
}