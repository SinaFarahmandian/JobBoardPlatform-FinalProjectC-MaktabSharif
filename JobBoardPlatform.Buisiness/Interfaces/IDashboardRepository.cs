using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IDashboardRepository
{
    Task<int> GetJobSeekerCountAsync();
    Task<int> GetEmployerCountAsync();
    Task<int> GetActiveJobPostingCountAsync();
    Task<int> GetInactiveJobPostingCountAsync();
    Task<Dictionary<ApplicationStatus, int>> GetApplicationCountsByStatusAsync();
    Task<int> GetPendingEmployerCountAsync();
}