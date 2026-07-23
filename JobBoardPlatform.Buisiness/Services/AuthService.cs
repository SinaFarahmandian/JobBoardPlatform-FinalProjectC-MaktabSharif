using System.Security.Cryptography;
using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.Auth;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities;
using JobBoardPlatform.Domain.Entities.Auth;
using JobBoardPlatform.Domain.Entities.Companies;
using JobBoardPlatform.Domain.Entities.Employers;
using JobBoardPlatform.Domain.Entities.JobSeekers;

namespace JobBoardPlatform.Buisiness.Services;

using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ICompanyRepository _companyRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private const int RefreshTokenExpiryDays = 30;

    public AuthService(UserManager<User> userManager, ICompanyRepository companyRepository, IJwtTokenService jwtTokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _companyRepository = companyRepository;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthResultDto> RegisterJobSeekerAsync(RegisterJobSeekerDto dto)
    {
        var jobSeeker = new JobSeeker(dto.FullName, dto.Email) { PhoneNumber = dto.PhoneNumber };
        var result = await _userManager.CreateAsync(jobSeeker, dto.Password);

        if (!result.Succeeded)
            return AuthResultDto.Failure(result.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(jobSeeker, "JobSeeker");
        return AuthResultDto.Success("ثبت‌نام با موفقیت انجام شد. اکنون می‌توانید وارد شوید.");
    }

    public async Task<AuthResultDto> RegisterEmployerAsync(RegisterEmployerDto dto)
    {
        var company = new Company(dto.CompanyName, dto.CompanyWebsite, dto.CompanyDescription, dto.Industry);
        await _companyRepository.AddAsync(company);

        var employer = new Employer(dto.FullName, dto.Email, company.Id) { PhoneNumber = dto.PhoneNumber };
        var result = await _userManager.CreateAsync(employer, dto.Password);

        if (!result.Succeeded)
            return AuthResultDto.Failure(result.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(employer, "Employer");
        return AuthResultDto.Success("ثبت‌نام با موفقیت انجام شد. حساب شما تا تأیید ادمین غیرفعال است.");
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return AuthResultDto.Failure(new[] { "ایمیل یا رمز عبور اشتباه است" });

        if (!user.IsActive)
            return AuthResultDto.Failure(new[] { "حساب شما غیرفعال شده است" });

        if (user is Employer && !user.IsApproved)
            return AuthResultDto.Failure(new[] { "حساب کارفرمایی شما هنوز توسط ادمین تأیید نشده است" });

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.GenerateToken(user, roles);
        var refreshToken = await GenerateAndStoreRefreshTokenAsync(user.Id);

        return AuthResultDto.SuccessWithToken(accessToken, refreshToken, user.FullName, user.Email!, roles.FirstOrDefault() ?? "");
    }

    public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (storedToken == null || !storedToken.IsActive)
            return AuthResultDto.Failure(new[] { "Refresh Token نامعتبر یا منقضی‌شده است" });

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString())
            ?? throw new NotFoundException("کاربر پیدا نشد");

        // توکن قدیمی باطل می‌شه (Rotation) و یکی جدید صادر می‌شه
        storedToken.Revoke();
        await _refreshTokenRepository.UpdateAsync(storedToken);

        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _jwtTokenService.GenerateToken(user, roles);
        var newRefreshToken = await GenerateAndStoreRefreshTokenAsync(user.Id);

        return AuthResultDto.SuccessWithToken(newAccessToken, newRefreshToken, user.FullName, user.Email!, roles.FirstOrDefault() ?? "");
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (storedToken != null && storedToken.IsActive)
        {
            storedToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(storedToken);
        }
    }

    private async Task<string> GenerateAndStoreRefreshTokenAsync(int userId)
    {
        var tokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken(userId, tokenValue, DateTime.UtcNow.AddDays(RefreshTokenExpiryDays));
        await _refreshTokenRepository.AddAsync(refreshToken);
        return tokenValue;
    }
}

// var claims = new List<Claim>
// {
//     new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
//     new (JwtRegisteredClaimNames.Email, user.Email.ToString()),
//     new (JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
// };
//
// var userClaims = await _userManager.GetClaimsAsync(user);
// claims.AddRange(userClaims);
//
// var roles = await _userManager.GetRolesAsync(user);
//
// foreach (var roleName in roles)
// {
//     claims.Add(new Claim(ClaimTypes.Role, roleName));
//
//     var role = await _roleManager.FindByNameAsync(roleName);
//
//     if (role is null)
//         continue;
//
//     var roleClaims = await _roleManager.GetClaimsAsync(role);
//     claims.AddRange(roleClaims);
// }
//
// // remove duplicate claims
// claims = claims.DistinctBy(c => (c.Type, c.Value)).ToList();