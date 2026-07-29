using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.Admin;

public class SetJobPostingStatusDto
{
    [Required]
    public bool IsActive { get; set; }
}