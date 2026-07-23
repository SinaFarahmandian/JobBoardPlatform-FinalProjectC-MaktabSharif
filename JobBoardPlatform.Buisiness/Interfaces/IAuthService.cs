using JobBoardPlatform.Buisiness.Dtos.Auth;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> RegisterJobSeekerAsync(RegisterJobSeekerDto dto);
    Task<AuthResultDto> RegisterEmployerAsync(RegisterEmployerDto dto);
    Task<AuthResultDto> LoginAsync(LoginDto dto);
    Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
}