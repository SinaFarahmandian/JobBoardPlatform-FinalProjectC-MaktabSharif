namespace JobBoardPlatform.Buisiness.Dtos.Admin;

public class JobPostingAdminDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string EmployerFullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? FeaturedUntil { get; set; }
    public DateTime CreatedAt { get; set; }
}