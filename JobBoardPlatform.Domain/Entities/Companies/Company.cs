using System.ComponentModel.DataAnnotations;
using JobBoardPlatform.Domain.Entities.Employers;

namespace JobBoardPlatform.Domain.Entities.Companies;

public class Company : BaseEntity
{
    [Required, StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Url, StringLength(300)]
    public string? Website { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Industry { get; set; }

    public ICollection<Employer> Employers { get; set; } = new List<Employer>();

    private Company() { }

    public Company(string name, string? website = null, string? description = null, string? industry = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The company name cannot be empty", nameof(name));

        Name = name;
        Website = website;
        Description = description;
        Industry = industry;
    }
}
