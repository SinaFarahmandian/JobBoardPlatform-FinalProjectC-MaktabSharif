using JobBoardPlatform.Buisiness.Dtos.Auth;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register/jobseeker")]
    public async Task<IActionResult> RegisterJobSeeker(RegisterJobSeekerDto dto)
    {
        var result = await _authService.RegisterJobSeekerAsync(dto);
        return result.Succeeded ? Ok(new { message = result.Message }) : BadRequest(new { errors = result.Errors });
    }

    [HttpPost("register/employer")]
    public async Task<IActionResult> RegisterEmployer(RegisterEmployerDto dto)
    {
        var result = await _authService.RegisterEmployerAsync(dto);
        return result.Succeeded ? Ok(new { message = result.Message }) : BadRequest(new { errors = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.Succeeded) return Unauthorized(new { errors = result.Errors });
        return Ok(new { token = result.Token, fullName = result.FullName, email = result.Email, role = result.Role });
    }
}