using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.MVC.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/employers")]
public class AdminEmployersController : Controller
{
    private readonly IAdminEmployerService _service;
    public AdminEmployersController(IAdminEmployerService service) => _service = service;

    [HttpGet("")]
    public async Task<IActionResult> Index(bool onlyPending = false) => View(await _service.GetAllAsync(onlyPending));

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id) => View(await _service.GetDetailsAsync(id));

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        await _service.ApproveAsync(id);
        TempData["Success"] = "The employer has been approved";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(int id)
    {
        await _service.RejectAsync(id);
        TempData["Success"] = "The employer has been rejected";
        return RedirectToAction(nameof(Index));
    }
}
