using JobBoardPlatform.Buisiness.Dtos.JobApplication;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/employer/applications")]
public class EmployerApplicationsController : EmployerControllerBase
{
    private readonly IEmployerApplicationService _service;
    public EmployerApplicationsController(IEmployerApplicationService service) => _service = service;

    [HttpGet("job-posting/{jobPostingId}")]
    public async Task<IActionResult> GetForJobPosting(int jobPostingId)
    {
        EnsureApproved();
        return Ok(await _service.GetApplicationsForJobPostingAsync(GetEmployerId(), jobPostingId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetails(int id)
    {
        EnsureApproved();
        return Ok(await _service.GetApplicationDetailsAsync(GetEmployerId(), id));
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, UpdateApplicationStatusDto dto)
    {
        EnsureApproved();
        return Ok(await _service.ChangeStatusAsync(GetEmployerId(), id, dto.NewStatus));
    }
}