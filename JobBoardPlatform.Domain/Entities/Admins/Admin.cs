using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities.Admins;

// Admin.cs
public class Admin : User
{
    public string AccessLevel { get; set; } = "Standard";

    private Admin() { }

    public Admin(string fullName, string email, string accessLevel = "Standard") : base(fullName, email)
    {
        Role = UserRole.Admin;
        AccessLevel = accessLevel;
    }
}