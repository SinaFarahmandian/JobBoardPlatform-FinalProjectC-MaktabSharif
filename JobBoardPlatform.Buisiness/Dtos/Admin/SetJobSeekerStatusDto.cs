using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.Admin;

public class SetJobSeekerStatusDto
{
    [Required]
    public bool IsActive { get; set; }
}