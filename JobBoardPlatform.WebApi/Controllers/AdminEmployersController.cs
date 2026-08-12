using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/admin/employers")]
public class AdminEmployersController : AdminControllerBase
{
    private readonly IAdminEmployerService _service;
    public AdminEmployersController(IAdminEmployerService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool onlyPending = false)
        => Ok(await _service.GetAllAsync(onlyPending));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetails(int id)
        => Ok(await _service.GetDetailsAsync(id));

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        await _service.ApproveAsync(id);
        return Ok(new { message = "The employer has been approved" });
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(int id)
    {
        await _service.RejectAsync(id);
        return Ok(new { message = "The employer has been rejected" });
    }
}
