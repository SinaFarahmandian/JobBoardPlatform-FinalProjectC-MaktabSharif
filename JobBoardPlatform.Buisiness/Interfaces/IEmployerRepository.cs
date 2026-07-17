using JobBoardPlatform.Domain.Entities.Employers;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IEmployerRepository
{
    Task<Employer?> GetByIdAsync(int id);
    Task<Employer?> GetByIdWithCompanyAsync(int id);
    Task<List<Employer>> GetPendingApprovalAsync();
    Task UpdateAsync(Employer employer);
}