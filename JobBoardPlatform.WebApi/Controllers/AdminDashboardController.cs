using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/admin/dashboard")]
public class AdminDashboardController : AdminControllerBase
{
    private readonly IAdminDashboardService _service;
    public AdminDashboardController(IAdminDashboardService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _service.GetStatsAsync());
}