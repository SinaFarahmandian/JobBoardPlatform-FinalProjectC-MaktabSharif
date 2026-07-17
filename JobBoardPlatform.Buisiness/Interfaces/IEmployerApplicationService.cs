using JobBoardPlatform.Buisiness.Dtos.JobApplication;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IEmployerApplicationService
{
    Task<List<JobApplicationDto>> GetApplicationsForJobPostingAsync(int employerId, int jobPostingId);
    Task<JobApplicationDto> GetApplicationDetailsAsync(int employerId, int applicationId);
    Task<JobApplicationDto> ChangeStatusAsync(int employerId, int applicationId, ApplicationStatus newStatus);
}
