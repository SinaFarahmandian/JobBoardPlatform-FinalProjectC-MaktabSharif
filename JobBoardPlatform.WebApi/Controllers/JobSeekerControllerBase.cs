using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[Authorize(Roles = "JobSeeker")]
public abstract class JobSeekerControllerBase : ControllerBase
{
    protected int GetJobSeekerId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}