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
        return Ok(new { token = result.Token, refreshToken = result.RefreshToken, fullName = result.FullName, email = result.Email, role = result.Role });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestDto dto)
    {
        var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
        if (!result.Succeeded) return Unauthorized(new { errors = result.Errors });
        return Ok(new { token = result.Token, refreshToken = result.RefreshToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenRequestDto dto)
    {
        await _authService.LogoutAsync(dto.RefreshToken);
        return Ok(new { message = "خروج با موفقیت انجام شد" });
    }
}