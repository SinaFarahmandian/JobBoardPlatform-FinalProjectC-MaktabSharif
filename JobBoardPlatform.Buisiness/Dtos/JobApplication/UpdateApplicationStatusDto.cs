using System.ComponentModel.DataAnnotations;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Buisiness.Dtos.JobApplication;

public class UpdateApplicationStatusDto
{
    [Required]
    public ApplicationStatus NewStatus { get; set; }
}