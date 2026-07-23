using JobBoardPlatform.Buisiness.Common;
using JobBoardPlatform.Buisiness.Dtos.JobPosting;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IPublicJobPostingService
{
    Task<PagedResult<JobPostingPublicDto>> SearchAsync(JobPostingSearchQueryDto query);
    Task<JobPostingPublicDto> GetByIdAsync(int id);
}