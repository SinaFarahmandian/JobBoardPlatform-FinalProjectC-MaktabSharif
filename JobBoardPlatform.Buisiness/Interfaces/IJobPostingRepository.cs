using JobBoardPlatform.Domain.Entities.JobPostings;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJobPostingRepository
{
    Task<JobPosting?> GetByIdAsync(int id);
    Task<List<JobPosting>> GetByEmployerIdAsync(int employerId);
    Task AddAsync(JobPosting posting);
    Task UpdateAsync(JobPosting posting);
    Task DeleteAsync(JobPosting posting);
}