using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.Admin;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobSeekers;

namespace JobBoardPlatform.Buisiness.Services;

public class AdminJobSeekerService : IAdminJobSeekerService
{
    private readonly IJobSeekerRepository _repo;
    private readonly IJobApplicationRepository _appRepo;

    public AdminJobSeekerService(IJobSeekerRepository repo, IJobApplicationRepository appRepo)
    {
        _repo = repo;
        _appRepo = appRepo;
    }

    public async Task<List<JobSeekerAdminDto>> GetAllAsync()
    {
        var seekers = await _repo.GetAllAsync();
        return seekers.Select(MapToDto).ToList();
    }

    public async Task<JobSeekerAdminDetailsDto> GetDetailsAsync(int jobSeekerId)
    {
        var seeker = await _repo.GetByIdAsync(jobSeekerId) ?? throw new NotFoundException("کارجو پیدا نشد");
        var applications = await _appRepo.GetByJobSeekerIdAsync(jobSeekerId);

        return new JobSeekerAdminDetailsDto
        {
            Id = seeker.Id, FullName = seeker.FullName, Email = seeker.Email!,
            IsActive = seeker.IsActive, CreatedAt = seeker.CreatedAt,
            Skills = seeker.Skills, YearsOfExperience = seeker.YearsOfExperience,
            DesiredJobTitle = seeker.DesiredJobTitle, HasResume = seeker.ResumeData != null,
            ApplicationsCount = applications.Count
        };
    }

    public async Task SetActiveStatusAsync(int jobSeekerId, bool isActive)
    {
        var seeker = await _repo.GetByIdAsync(jobSeekerId) ?? throw new NotFoundException("کارجو پیدا نشد");
        seeker.IsActive = isActive;
        seeker.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(seeker);
    }

    private static JobSeekerAdminDto MapToDto(JobSeeker s) => new()
    {
        Id = s.Id, FullName = s.FullName, Email = s.Email!, IsActive = s.IsActive, CreatedAt = s.CreatedAt
    };
}