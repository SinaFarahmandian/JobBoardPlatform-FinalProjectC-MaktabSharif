using JobBoardPlatform.Domain.Entities.Auth;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task UpdateAsync(RefreshToken token);
}