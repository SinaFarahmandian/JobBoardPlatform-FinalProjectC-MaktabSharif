using JobBoardPlatform.Buisiness.Dtos.JobPosting;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJobPostingService
{
    Task<JobPostingDto> CreateAsync(int employerId, CreateJobPostingDto dto);
    Task<JobPostingDto> UpdateAsync(int employerId, int jobPostingId, UpdateJobPostingDto dto);
    Task DeleteAsync(int employerId, int jobPostingId);
    Task<List<JobPostingDto>> GetMyJobPostingsAsync(int employerId);
    Task<JobPostingDto> GetByIdAsync(int employerId, int jobPostingId);
    Task<JobPostingDto> ToggleActiveAsync(int employerId, int jobPostingId);
}