using JobBoardPlatform.Domain.Entities;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user, IList<string> roles);
}