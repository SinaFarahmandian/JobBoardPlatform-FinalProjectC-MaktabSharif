using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.Company;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.Companies;

namespace JobBoardPlatform.Buisiness.Services;

public class CompanyService : ICompanyService
{
    private readonly IEmployerRepository _employerRepository;
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(IEmployerRepository employerRepository, ICompanyRepository companyRepository)
    {
        _employerRepository = employerRepository;
        _companyRepository = companyRepository;
    }

    public async Task<CompanyDto> GetMyCompanyAsync(int employerId)
    {
        var employer = await _employerRepository.GetByIdWithCompanyAsync(employerId)
                       ?? throw new NotFoundException("The employer was not found");
        return MapToDto(employer.Company);
    }

    public async Task<CompanyDto> UpdateMyCompanyAsync(int employerId, UpdateCompanyDto dto)
    {
        var employer = await _employerRepository.GetByIdWithCompanyAsync(employerId)
                       ?? throw new NotFoundException("The employer was not found");

        employer.Company.Name = dto.Name;
        employer.Company.Website = dto.Website;
        employer.Company.Description = dto.Description;
        employer.Company.Industry = dto.Industry;
        employer.Company.UpdatedAt = DateTime.UtcNow;

        await _companyRepository.UpdateAsync(employer.Company);
        return MapToDto(employer.Company);
    }

    private static CompanyDto MapToDto(Company c) => new()
    {
        Id = c.Id, Name = c.Name, Website = c.Website, Description = c.Description, Industry = c.Industry
    };
}
