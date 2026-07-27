using JobBoardPlatform.Buisiness.Dtos.JobSeekerProfile;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/jobseeker/profile")]
public class JobSeekerProfileController : JobSeekerControllerBase
{
    private readonly IJobSeekerProfileService _service;
    public JobSeekerProfileController(IJobSeekerProfileService service) => _service = service;

    [HttpGet("me")]
    public async Task<IActionResult> GetMine() => Ok(await _service.GetMyProfileAsync(GetJobSeekerId()));

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMine(UpdateJobSeekerProfileDto dto)
        => Ok(await _service.UpdateMyProfileAsync(GetJobSeekerId(), dto));

    [HttpPost("resume")]
    public async Task<IActionResult> UploadResume(IFormFile file)
        => Ok(await _service.UploadResumeAsync(GetJobSeekerId(), file));

    [HttpGet("resume")]
    public async Task<IActionResult> DownloadResume()
    {
        var (data, contentType, fileName) = await _service.GetResumeAsync(GetJobSeekerId());
        return File(data, contentType, fileName);
    }

    [HttpDelete("resume")]
    public async Task<IActionResult> DeleteResume()
        => Ok(await _service.DeleteResumeAsync(GetJobSeekerId()));
}