using JobBoardPlatform.Buisiness.Dtos.JobApplication;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IAdminService
{
    Task ApproveEmployerAsync(int employerId);
    Task<List<EmployerSummaryDto>> GetPendingEmployersAsync();
}
