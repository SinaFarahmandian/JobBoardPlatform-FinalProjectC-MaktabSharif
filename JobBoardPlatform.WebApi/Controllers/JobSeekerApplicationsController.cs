using JobBoardPlatform.Buisiness.Dtos.JobApplication;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/jobseeker/applications")]
public class JobSeekerApplicationsController : JobSeekerControllerBase
{
    private readonly IJobSeekerApplicationService _service;
    public JobSeekerApplicationsController(IJobSeekerApplicationService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Apply(CreateJobApplicationDto dto)
        => Ok(await _service.ApplyAsync(GetJobSeekerId(), dto));

    [HttpGet]
    public async Task<IActionResult> GetMine()
        => Ok(await _service.GetMyApplicationsAsync(GetJobSeekerId()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetails(int id)
        => Ok(await _service.GetDetailsAsync(GetJobSeekerId(), id));

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
        => Ok(await _service.CancelAsync(GetJobSeekerId(), id));
}