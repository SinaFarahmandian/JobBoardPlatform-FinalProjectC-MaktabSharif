using JobBoardPlatform.Buisiness.Dtos.Admin;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IAdminJobSeekerService
{
    Task<List<JobSeekerAdminDto>> GetAllAsync();
    Task<JobSeekerAdminDetailsDto> GetDetailsAsync(int jobSeekerId);
    Task SetActiveStatusAsync(int jobSeekerId, bool isActive);
}