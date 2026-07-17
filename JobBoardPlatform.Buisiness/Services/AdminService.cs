using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.JobApplication;
using JobBoardPlatform.Buisiness.Interfaces;

namespace JobBoardPlatform.Buisiness.Services;

// AdminService.cs
public class AdminService : IAdminService
{
    private readonly IEmployerRepository _employerRepository;
    public AdminService(IEmployerRepository employerRepository) => _employerRepository = employerRepository;

    public async Task ApproveEmployerAsync(int employerId)
    {
        var employer = await _employerRepository.GetByIdAsync(employerId) ?? throw new NotFoundException("کارفرما پیدا نشد");
        employer.IsApproved = true;
        employer.UpdatedAt = DateTime.UtcNow;
        await _employerRepository.UpdateAsync(employer);
    }

    public async Task<List<EmployerSummaryDto>> GetPendingEmployersAsync()
    {
        var employers = await _employerRepository.GetPendingApprovalAsync();
        return employers.Select(e => new EmployerSummaryDto
        {
            Id = e.Id, FullName = e.FullName, Email = e.Email!, CompanyName = e.Company?.Name ?? ""
        }).ToList();
    }
}