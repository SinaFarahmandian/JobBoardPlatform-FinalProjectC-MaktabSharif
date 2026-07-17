using JobBoardPlatform.Buisiness.Dtos.Company;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/company")]
public class CompanyController : EmployerControllerBase
{
    private readonly ICompanyService _companyService;
    public CompanyController(ICompanyService companyService) => _companyService = companyService;

    [HttpGet("me")]
    public async Task<IActionResult> GetMine()
    {
        EnsureApproved();
        return Ok(await _companyService.GetMyCompanyAsync(GetEmployerId()));
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMine(UpdateCompanyDto dto)
    {
        EnsureApproved();
        return Ok(await _companyService.UpdateMyCompanyAsync(GetEmployerId(), dto));
    }
}