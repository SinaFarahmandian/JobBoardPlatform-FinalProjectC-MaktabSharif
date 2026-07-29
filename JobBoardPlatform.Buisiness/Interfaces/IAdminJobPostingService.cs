using JobBoardPlatform.Buisiness.Dtos.Admin;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IAdminJobPostingService
{
    Task<List<JobPostingAdminDto>> GetAllAsync();
    Task SetActiveStatusAsync(int jobPostingId, bool isActive);
    Task DeleteAsync(int jobPostingId);
    Task SetFeaturedAsync(int jobPostingId, SetFeaturedDto dto);
}