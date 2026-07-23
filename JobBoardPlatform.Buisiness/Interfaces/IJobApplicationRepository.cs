using JobBoardPlatform.Domain.Entities.JobApplications;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(int id);
    Task<List<JobApplication>> GetByJobPostingIdAsync(int jobPostingId);
    Task<List<JobApplication>> GetByJobSeekerIdAsync(int jobSeekerId);
    Task<bool> ExistsAsync(int jobSeekerId, int jobPostingId);
    Task AddAsync(JobApplication application);
    Task UpdateAsync(JobApplication application);
}