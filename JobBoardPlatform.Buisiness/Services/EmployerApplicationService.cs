using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.JobApplication;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobApplications;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Buisiness.Services;

public class EmployerApplicationService : IEmployerApplicationService
{
    private readonly IJobApplicationRepository _appRepo;
    private readonly IJobPostingRepository _postingRepo;

    private static readonly Dictionary<ApplicationStatus, ApplicationStatus[]> AllowedTransitions = new()
    {
        [ApplicationStatus.Pending] = new[] { ApplicationStatus.Reviewing },
        [ApplicationStatus.Reviewing] = new[] { ApplicationStatus.Interview, ApplicationStatus.Rejected },
        [ApplicationStatus.Interview] = new[] { ApplicationStatus.Accepted, ApplicationStatus.Rejected },
    };

    public EmployerApplicationService(IJobApplicationRepository appRepo, IJobPostingRepository postingRepo)
    {
        _appRepo = appRepo;
        _postingRepo = postingRepo;
    }

    public async Task<List<JobApplicationDto>> GetApplicationsForJobPostingAsync(int employerId, int jobPostingId)
    {
        await EnsureOwnsPostingAsync(employerId, jobPostingId);
        var apps = await _appRepo.GetByJobPostingIdAsync(jobPostingId);
        return apps.Select(MapToDto).ToList();
    }

    public async Task<JobApplicationDto> GetApplicationDetailsAsync(int employerId, int applicationId)
        => MapToDto(await GetOwnedApplicationAsync(employerId, applicationId));

    public async Task<JobApplicationDto> ChangeStatusAsync(int employerId, int applicationId, ApplicationStatus newStatus)
    {
        var app = await GetOwnedApplicationAsync(employerId, applicationId);

        if (!AllowedTransitions.TryGetValue(app.Status, out var allowed) || !allowed.Contains(newStatus))
            throw new InvalidStatusTransitionException($"تغییر وضعیت از {app.Status} به {newStatus} مجاز نیست");

        app.Status = newStatus;
        app.UpdatedAt = DateTime.UtcNow;
        await _appRepo.UpdateAsync(app);
        return MapToDto(app);
    }

    private async Task EnsureOwnsPostingAsync(int employerId, int jobPostingId)
    {
        var posting = await _postingRepo.GetByIdAsync(jobPostingId) ?? throw new NotFoundException("آگهی پیدا نشد");
        if (posting.EmployerId != employerId)
            throw new ForbiddenAccessException("شما به این آگهی دسترسی ندارید");
    }

    private async Task<JobApplication> GetOwnedApplicationAsync(int employerId, int applicationId)
    {
        var app = await _appRepo.GetByIdAsync(applicationId) ?? throw new NotFoundException("درخواست پیدا نشد");
        if (app.JobPosting.EmployerId != employerId)
            throw new ForbiddenAccessException("شما به این درخواست دسترسی ندارید");
        return app;
    }

    private static JobApplicationDto MapToDto(JobApplication ja) => new()
    {
        Id = ja.Id, JobPostingId = ja.JobPostingId, JobPostingTitle = ja.JobPosting?.Title ?? "",
        JobSeekerId = ja.JobSeekerId, JobSeekerName = ja.JobSeeker?.FullName ?? "",
        Status = ja.Status.ToString(), CoverLetter = ja.CoverLetter, AppliedAt = ja.CreatedAt
    };
}