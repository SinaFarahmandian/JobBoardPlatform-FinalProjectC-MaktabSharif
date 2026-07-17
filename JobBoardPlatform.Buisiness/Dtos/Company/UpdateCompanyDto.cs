using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.Company;

public class UpdateCompanyDto
{
    [Required, StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Url]
    public string? Website { get; set; }
    public string? Description { get; set; }
    public string? Industry { get; set; }
}