using JobBoardPlatform.Buisiness.Dtos.Admin;
using JobBoardPlatform.Buisiness.Interfaces;

namespace JobBoardPlatform.Buisiness.Services;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IDashboardRepository _repo;
    public AdminDashboardService(IDashboardRepository repo) => _repo = repo;

    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var applicationCounts = await _repo.GetApplicationCountsByStatusAsync();

        return new DashboardStatsDto
        {
            TotalJobSeekers = await _repo.GetJobSeekerCountAsync(),
            TotalEmployers = await _repo.GetEmployerCountAsync(),
            ActiveJobPostings = await _repo.GetActiveJobPostingCountAsync(),
            InactiveJobPostings = await _repo.GetInactiveJobPostingCountAsync(),
            ApplicationsByStatus = applicationCounts.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value),
            PendingEmployerApprovals = await _repo.GetPendingEmployerCountAsync()
        };
    }
}