using System.Security.Claims;
using JobBoardPlatform.Buisiness.Dtos.Company;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "Employer")]
public class CompanyController : Controller
{
    private readonly ICompanyService _service;
    public CompanyController(ICompanyService service) => _service = service;

    private int EmployerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> Index() => View(await _service.GetMyCompanyAsync(EmployerId));

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var c = await _service.GetMyCompanyAsync(EmployerId);
        return View(new UpdateCompanyDto { Name = c.Name, Website = c.Website, Description = c.Description, Industry = c.Industry });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateCompanyDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _service.UpdateMyCompanyAsync(EmployerId, dto);
        TempData["Success"] = "The company profile has been updated";
        return RedirectToAction(nameof(Index));
    }
}
