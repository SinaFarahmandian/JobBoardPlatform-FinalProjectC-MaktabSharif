using JobBoardPlatform.Buisiness.Dtos.Admin;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/admin/job-postings")]
public class AdminJobPostingsController : AdminControllerBase
{
    private readonly IAdminJobPostingService _service;
    public AdminJobPostingsController(IAdminJobPostingService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> SetStatus(int id, SetJobPostingStatusDto dto)
    {
        await _service.SetActiveStatusAsync(id, dto.IsActive);
        return Ok(new { message = "وضعیت آگهی به‌روزرسانی شد" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/featured")]
    public async Task<IActionResult> SetFeatured(int id, SetFeaturedDto dto)
    {
        await _service.SetFeaturedAsync(id, dto);
        return Ok(new { message = "وضعیت ویژه‌بودن آگهی به‌روزرسانی شد" });
    }
}