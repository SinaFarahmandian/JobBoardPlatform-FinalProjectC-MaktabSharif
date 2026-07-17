using JobBoardPlatform.Domain.Entities.JobApplications;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(int id);
    Task<List<JobApplication>> GetByJobPostingIdAsync(int jobPostingId);
    Task UpdateAsync(JobApplication application);
}