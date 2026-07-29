using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.Admin;

public class SetFeaturedDto
{
    [Required]
    public bool IsFeatured { get; set; }
    public DateTime? FeaturedUntil { get; set; }
}