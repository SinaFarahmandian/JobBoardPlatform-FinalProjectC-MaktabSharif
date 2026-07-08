using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities.Admins;

public class Admin : User
{
    public Admin() => Role = UserRole.Admin;

    public string? AccessLevel { get; set; }
}