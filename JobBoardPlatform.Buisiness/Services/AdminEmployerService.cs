using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.Admin;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.Employers;

namespace JobBoardPlatform.Buisiness.Services;

public class AdminEmployerService : IAdminEmployerService
{
    private readonly IEmployerRepository _employerRepo;
    private readonly IEmailNotificationService _emailNotifier;

    public AdminEmployerService(IEmployerRepository employerRepo, IEmailNotificationService emailNotifier)
    {
        _employerRepo = employerRepo;
        _emailNotifier = emailNotifier;
    }

    public async Task<List<EmployerAdminDto>> GetAllAsync(bool onlyPending)
    {
        var employers = onlyPending
            ? await _employerRepo.GetPendingApprovalAsync()
            : await _employerRepo.GetAllAsync();

        return employers.Select(MapToDto).ToList();
    }

    public async Task<EmployerAdminDetailsDto> GetDetailsAsync(int employerId)
    {
        var employer = await _employerRepo.GetByIdWithCompanyAsync(employerId)
            ?? throw new NotFoundException("کارفرما پیدا نشد");

        return new EmployerAdminDetailsDto
        {
            Id = employer.Id, FullName = employer.FullName, Email = employer.Email!,
            IsApproved = employer.IsApproved, IsActive = employer.IsActive,
            CompanyName = employer.Company.Name, CreatedAt = employer.CreatedAt,
            CompanyWebsite = employer.Company.Website, CompanyDescription = employer.Company.Description,
            Industry = employer.Company.Industry, JobPostingsCount = employer.JobPostings?.Count ?? 0
        };
    }

    public async Task ApproveAsync(int employerId)
    {
        var employer = await _employerRepo.GetByIdWithCompanyAsync(employerId)
                       ?? throw new NotFoundException("کارفرما پیدا نشد");

        if (employer.IsApproved)
            throw new BadRequestException("این کارفرما از قبل تأیید شده است");

        employer.IsApproved = true;
        employer.UpdatedAt = DateTime.UtcNow;
        await _employerRepo.UpdateAsync(employer);

        await _emailNotifier.SendAsync("EmployerApproved", employer.Email!, new Dictionary<string, string>
        {
            ["FullName"] = employer.FullName,
            ["CompanyName"] = employer.Company.Name
        });
    }

    public async Task RejectAsync(int employerId)
    {
        var employer = await _employerRepo.GetByIdWithCompanyAsync(employerId)
                       ?? throw new NotFoundException("کارفرما پیدا نشد");

        if (!employer.IsApproved)
            throw new BadRequestException("این کارفرما از قبل تأییدنشده/ردشده است");

        employer.IsApproved = false;
        employer.UpdatedAt = DateTime.UtcNow;
        await _employerRepo.UpdateAsync(employer);

        await _emailNotifier.SendAsync("EmployerRejected", employer.Email!, new Dictionary<string, string>
        {
            ["FullName"] = employer.FullName,
            ["CompanyName"] = employer.Company.Name
        });
    }

    private static EmployerAdminDto MapToDto(Employer e) => new()
    {
        Id = e.Id, FullName = e.FullName, Email = e.Email!, IsApproved = e.IsApproved,
        IsActive = e.IsActive, CompanyName = e.Company?.Name ?? "", CreatedAt = e.CreatedAt
    };
}