using JobBoardPlatform.Buisiness.Dtos.JobPosting;
using JobBoardPlatform.Domain.Entities.JobPostings;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJobPostingRepository
{
    Task<List<JobPosting>> GetAllForAdminAsync();
    Task<JobPosting?> GetByIdAsync(int id);
    Task<List<JobPosting>> GetByEmployerIdAsync(int employerId);
    Task AddAsync(JobPosting posting);
    Task UpdateAsync(JobPosting posting);
    Task DeleteAsync(JobPosting posting);
    Task<(List<JobPosting> Items, int TotalCount)> SearchActiveAsync(JobPostingSearchQueryDto query);
    Task<JobPosting?> GetActiveByIdAsync(int id);
}