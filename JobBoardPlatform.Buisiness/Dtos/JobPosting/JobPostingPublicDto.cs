namespace JobBoardPlatform.Buisiness.Dtos.JobPosting;

public class JobPostingPublicDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string EmploymentType { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Skills { get; set; }
    public bool IsFeatured { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}