using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.JobPosting;

public class CreateJobPostingDto : IValidatableObject
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
    
    [StringLength(100)]
    public string? Category { get; set; }

    [StringLength(500)]
    public string? Skills { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SalaryMin.HasValue && SalaryMax.HasValue && SalaryMax < SalaryMin)
            yield return new ValidationResult("The maximum salary cannot be lower than the minimum salary", new[] { nameof(SalaryMax) });

        if (ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.Now)
            yield return new ValidationResult("The application deadline must be in the future", new[] { nameof(ExpiresAt) });
    }
}


