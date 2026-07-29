using JobBoardPlatform.Buisiness.Dtos.Admin;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IAdminEmployerService
{
    Task<List<EmployerAdminDto>> GetAllAsync(bool onlyPending);
    Task<EmployerAdminDetailsDto> GetDetailsAsync(int employerId);
    Task ApproveAsync(int employerId);
    Task RejectAsync(int employerId);
}