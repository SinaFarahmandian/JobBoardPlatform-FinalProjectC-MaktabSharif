using JobBoardPlatform.Buisiness.Dtos.JobPosting;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/job-postings")]
public class JobPostingsController : EmployerControllerBase
{
    private readonly IJobPostingService _service;
    public JobPostingsController(IJobPostingService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(CreateJobPostingDto dto)
    {
        EnsureApproved();
        var result = await _service.CreateAsync(GetEmployerId(), dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateJobPostingDto dto)
    {
        EnsureApproved();
        return Ok(await _service.UpdateAsync(GetEmployerId(), id, dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        EnsureApproved();
        await _service.DeleteAsync(GetEmployerId(), id);
        return NoContent();
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        EnsureApproved();
        return Ok(await _service.GetMyJobPostingsAsync(GetEmployerId()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        EnsureApproved();
        return Ok(await _service.GetByIdAsync(GetEmployerId(), id));
    }

    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        EnsureApproved();
        return Ok(await _service.ToggleActiveAsync(GetEmployerId(), id));
    }
}