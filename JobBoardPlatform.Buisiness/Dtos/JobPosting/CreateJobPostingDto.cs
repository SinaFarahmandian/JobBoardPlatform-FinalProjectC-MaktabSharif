using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.JobPosting;

public class CreateJobPostingDto
{
    [Required, StringLength(150, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(5000, MinimumLength = 20)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Location { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal? SalaryMin { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalaryMax { get; set; }

    [Required, StringLength(50)]
    public string EmploymentType { get; set; } = "FullTime";

    public DateTime? ExpiresAt { get; set; }
}


