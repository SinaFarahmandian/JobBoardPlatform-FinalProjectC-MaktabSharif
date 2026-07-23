using JobBoardPlatform.Buisiness.Dtos.JobPosting;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/public/job-postings")]
public class PublicJobPostingsController : ControllerBase
{
    private readonly IPublicJobPostingService _service;
    public PublicJobPostingsController(IPublicJobPostingService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] JobPostingSearchQueryDto query)
        => Ok(await _service.SearchAsync(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));
}