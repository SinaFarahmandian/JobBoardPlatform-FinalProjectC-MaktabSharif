using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.Admin;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities;
using JobBoardPlatform.Domain.Entities.JobSeekers;
using Microsoft.AspNetCore.Identity;

namespace JobBoardPlatform.Buisiness.Services;

public class AdminJobSeekerService : IAdminJobSeekerService
{
    private readonly IJobSeekerRepository _repo;
    private readonly IJobApplicationRepository _appRepo;
    private readonly UserManager<User> _userManager;

    public AdminJobSeekerService(
        IJobSeekerRepository repo,
        IJobApplicationRepository appRepo,
        UserManager<User> userManager)
    {
        _repo = repo;
        _appRepo = appRepo;
        _userManager = userManager;
    }

    public async Task<List<JobSeekerAdminDto>> GetAllAsync()
    {
        var seekers = await _repo.GetAllAsync();
        return seekers.Select(MapToDto).ToList();
    }

    public async Task<JobSeekerAdminDetailsDto> GetDetailsAsync(int jobSeekerId)
    {
        var seeker = await _repo.GetByIdAsync(jobSeekerId) ?? throw new NotFoundException("The job seeker was not found");
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
        var seeker = await _repo.GetByIdAsync(jobSeekerId) ?? throw new NotFoundException("The job seeker was not found");
        seeker.IsActive = isActive;
        seeker.UpdatedAt = DateTime.UtcNow;
        var result = await _userManager.UpdateAsync(seeker);
        if (!result.Succeeded)
            throw new BadRequestException(string.Join(" ", result.Errors.Select(error => error.Description)));
    }

    private static JobSeekerAdminDto MapToDto(JobSeeker s) => new()
    {
        Id = s.Id, FullName = s.FullName, Email = s.Email!, IsActive = s.IsActive, CreatedAt = s.CreatedAt
    };
}
