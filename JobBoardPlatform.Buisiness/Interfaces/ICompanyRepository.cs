using JobBoardPlatform.Domain.Entities.Companies;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(int id);
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
    Task DeleteAsync(Company company);
}