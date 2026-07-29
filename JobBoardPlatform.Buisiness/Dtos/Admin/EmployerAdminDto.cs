namespace JobBoardPlatform.Buisiness.Dtos.Admin;

public class EmployerAdminDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public bool IsActive { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}