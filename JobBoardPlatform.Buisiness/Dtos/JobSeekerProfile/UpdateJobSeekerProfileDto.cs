using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.JobSeekerProfile;

public class UpdateJobSeekerProfileDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    [StringLength(1000)]
    public string? Skills { get; set; }

    [Range(0, 60)]
    public int YearsOfExperience { get; set; }

    [StringLength(150)]
    public string? DesiredJobTitle { get; set; }
}