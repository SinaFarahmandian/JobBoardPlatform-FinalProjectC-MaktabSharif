using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.JobApplication;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobApplications;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Buisiness.Services;

public class JobSeekerApplicationService : IJobSeekerApplicationService
{
    private readonly IJobApplicationRepository _appRepo;
    private readonly IJobPostingRepository _postingRepo;
    private readonly IEmailNotificationService _emailNotifier;

    public JobSeekerApplicationService(IJobApplicationRepository appRepo, IJobPostingRepository postingRepo, IEmailNotificationService emailNotifier)
    {
        _appRepo = appRepo;
        _postingRepo = postingRepo;
        _emailNotifier = emailNotifier;
    }

    public async Task<JobSeekerApplicationDto> ApplyAsync(int jobSeekerId, CreateJobApplicationDto dto)
    {
        var posting = await _postingRepo.GetByIdAsync(dto.JobPostingId)
            ?? throw new NotFoundException("The job posting was not found");

        if (!posting.IsActive)
            throw new BadRequestException("This job posting is no longer active");

        if (await _appRepo.ExistsAsync(jobSeekerId, dto.JobPostingId))
            throw new BadRequestException("You have already applied for this job");

        var application = new JobApplication(dto.JobPostingId, jobSeekerId, dto.CoverLetter);
        await _appRepo.AddAsync(application);
        
        await _emailNotifier.SendAsync("NewApplicationReceived", posting.Employer.Email!, new Dictionary<string, string>
        {
            ["EmployerName"] = posting.Employer.FullName,
            ["JobTitle"] = posting.Title
        });

        return await GetDetailsAsync(jobSeekerId, application.Id);
    }

    public async Task<List<JobSeekerApplicationDto>> GetMyApplicationsAsync(int jobSeekerId)
    {
        var apps = await _appRepo.GetByJobSeekerIdAsync(jobSeekerId);
        return apps.Select(MapToDto).ToList();
    }

    public async Task<JobSeekerApplicationDto> GetDetailsAsync(int jobSeekerId, int applicationId)
        => MapToDto(await GetOwnedAsync(jobSeekerId, applicationId));

    public async Task<JobSeekerApplicationDto> CancelAsync(int jobSeekerId, int applicationId)
    {
        var app = await GetOwnedAsync(jobSeekerId, applicationId);

        if (app.Status != ApplicationStatus.Pending)
            throw new BadRequestException("Only an application with Pending status can be withdrawn");

        app.Status = ApplicationStatus.Cancelled;
        app.UpdatedAt = DateTime.UtcNow;
        await _appRepo.UpdateAsync(app);

        return MapToDto(app);
    }

    private async Task<JobApplication> GetOwnedAsync(int jobSeekerId, int applicationId)
    {
        var app = await _appRepo.GetByIdAsync(applicationId) ?? throw new NotFoundException("The application was not found");
        if (app.JobSeekerId != jobSeekerId)
            throw new ForbiddenAccessException("You do not have access to this application");
        return app;
    }

    private static JobSeekerApplicationDto MapToDto(JobApplication ja) => new()
    {
        Id = ja.Id, JobPostingId = ja.JobPostingId, JobPostingTitle = ja.JobPosting?.Title ?? "",
        CompanyName = ja.JobPosting?.Employer?.Company?.Name ?? "",
        Status = ja.Status.ToString(), AppliedAt = ja.CreatedAt
    };
}
