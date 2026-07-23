using JobBoardPlatform.Buisiness.Dtos.JobApplication;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJobSeekerApplicationService
{
    Task<JobSeekerApplicationDto> ApplyAsync(int jobSeekerId, CreateJobApplicationDto dto);
    Task<List<JobSeekerApplicationDto>> GetMyApplicationsAsync(int jobSeekerId);
    Task<JobSeekerApplicationDto> GetDetailsAsync(int jobSeekerId, int applicationId);
    Task<JobSeekerApplicationDto> CancelAsync(int jobSeekerId, int applicationId);
}