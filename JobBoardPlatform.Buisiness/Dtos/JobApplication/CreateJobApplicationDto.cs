using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.JobApplication;

public class CreateJobApplicationDto
{
    [Required]
    public int JobPostingId { get; set; }

    [StringLength(3000)]
    public string? CoverLetter { get; set; }
}