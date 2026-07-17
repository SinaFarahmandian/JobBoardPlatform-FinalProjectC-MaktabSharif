using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    public AdminController(IAdminService adminService) => _adminService = adminService;

    [HttpGet("employers/pending")]
    public async Task<IActionResult> GetPending() => Ok(await _adminService.GetPendingEmployersAsync());

    [HttpPost("employers/{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        await _adminService.ApproveEmployerAsync(id);
        return Ok(new { message = "کارفرما با موفقیت تأیید شد" });
    }
}