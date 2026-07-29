using JobBoardPlatform.Buisiness.Dtos.Email;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/admin/email-templates")]
public class AdminEmailTemplatesController : AdminControllerBase
{
    private readonly IAdminEmailTemplateService _service;
    public AdminEmailTemplatesController(IAdminEmailTemplateService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpPut("{key}")]
    public async Task<IActionResult> Update(string key, UpdateEmailTemplateDto dto)
        => Ok(await _service.UpdateAsync(key, dto));
}