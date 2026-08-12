using JobBoardPlatform.Buisiness.Common;
using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.JobPosting;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobPostings;

namespace JobBoardPlatform.Buisiness.Services;

public class PublicJobPostingService : IPublicJobPostingService
{
    private readonly IJobPostingRepository _repo;
    public PublicJobPostingService(IJobPostingRepository repo) => _repo = repo;

    public async Task<PagedResult<JobPostingPublicDto>> SearchAsync(JobPostingSearchQueryDto query)
    {
        var (items, totalCount) = await _repo.SearchActiveAsync(query);

        return new PagedResult<JobPostingPublicDto>
        {
            Items = items.Select(MapToDto).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<JobPostingPublicDto> GetByIdAsync(int id)
    {
        var posting = await _repo.GetActiveByIdAsync(id)
                      ?? throw new NotFoundException("The job posting was not found or is no longer active");
        return MapToDto(posting);
    }

    private static JobPostingPublicDto MapToDto(JobPosting p) => new()
    {
        Id = p.Id, Title = p.Title, Description = p.Description, Location = p.Location,
        SalaryMin = p.SalaryMin, SalaryMax = p.SalaryMax, EmploymentType = p.EmploymentType,
        Category = p.Category, Skills = p.Skills,
        IsFeatured = p.IsFeatured && (p.FeaturedUntil == null || p.FeaturedUntil > DateTime.UtcNow),
        CompanyName = p.Employer?.Company?.Name ?? "", CreatedAt = p.CreatedAt
    };
}
