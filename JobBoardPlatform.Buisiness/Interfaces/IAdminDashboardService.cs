using JobBoardPlatform.Buisiness.Dtos.Admin;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IAdminDashboardService
{
    Task<DashboardStatsDto> GetStatsAsync();
}