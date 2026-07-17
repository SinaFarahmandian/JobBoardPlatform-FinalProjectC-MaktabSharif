using JobBoardPlatform.Buisiness.Dtos.Company;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface ICompanyService
{
    Task<CompanyDto> GetMyCompanyAsync(int employerId);
    Task<CompanyDto> UpdateMyCompanyAsync(int employerId, UpdateCompanyDto dto);
}