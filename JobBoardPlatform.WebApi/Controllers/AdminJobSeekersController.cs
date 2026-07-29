using JobBoardPlatform.Buisiness.Dtos.Admin;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/admin/jobseekers")]
public class AdminJobSeekersController : AdminControllerBase
{
    private readonly IAdminJobSeekerService _service;
    public AdminJobSeekersController(IAdminJobSeekerService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetails(int id) => Ok(await _service.GetDetailsAsync(id));

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> SetStatus(int id, SetJobSeekerStatusDto dto)
    {
        await _service.SetActiveStatusAsync(id, dto.IsActive);
        return Ok(new { message = "وضعیت کارجو به‌روزرسانی شد" });
    }
}