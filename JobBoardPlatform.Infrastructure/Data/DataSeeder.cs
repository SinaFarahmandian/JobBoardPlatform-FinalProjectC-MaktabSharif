using Bogus;
using JobBoardPlatform.Domain.Entities;
using JobBoardPlatform.Domain.Entities.Companies;
using JobBoardPlatform.Domain.Entities.Employers;
using JobBoardPlatform.Domain.Entities.JobPostings;
using JobBoardPlatform.Domain.Entities.JobSeekers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedFakeDataAsync(AppDbContext context, UserManager<User> userManager)
    {
        if (await context.Companies.AnyAsync())
            return;

        Randomizer.Seed = new Random(42);
        var faker = new Faker(); 

       
        var companyFaker = new Faker<Company>()
            .CustomInstantiator(f => new Company(
                f.Company.CompanyName(),
                f.Internet.Url(),
                f.Company.CatchPhrase(),
                f.Commerce.Department()
            ));

        var companies = companyFaker.Generate(10);
        context.Companies.AddRange(companies);
        await context.SaveChangesAsync();

        var employers = new List<Employer>();
        foreach (var company in companies)
        {
            var fullName = faker.Name.FullName();
            var email = faker.Internet.Email();

            var employer = new Employer(fullName, email, company.Id);
            var result = await userManager.CreateAsync(employer, "Test@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(employer, "Employer");
                employer.IsApproved = true;
                await userManager.UpdateAsync(employer);
                employers.Add(employer);
            }
        }

        var employmentTypes = new[] { "FullTime", "PartTime", "Remote", "Contract" };
        var categories = new[] { "IT", "Marketing", "Design", "Sales", "HR" };
        var skillsPool = new[] { "C#", "SQL", "React", "Python", "Docker", "Figma" };

        var jobPostingFaker = new Faker<JobPosting>()
            .CustomInstantiator(f =>
            {
                var employer = f.PickRandom(employers);
                return new JobPosting(
                    f.Name.JobTitle(),
                    f.Lorem.Paragraph(3),
                    f.Address.City(),
                    employer.Id,
                    f.PickRandom(employmentTypes)
                )
                {
                    SalaryMin = f.Random.Int(15, 30) * 1_000_000,
                    SalaryMax = f.Random.Int(31, 60) * 1_000_000,
                    Category = f.PickRandom(categories),
                    Skills = string.Join(",", f.PickRandom(skillsPool, f.Random.Int(2, 4))),
                    IsFeatured = f.Random.Bool(0.2f)
                };
            });

        var postings = jobPostingFaker.Generate(50);
        context.JobPostings.AddRange(postings);
        await context.SaveChangesAsync();

        for (int i = 0; i < 30; i++)
        {
            var fullName = faker.Name.FullName();
            var email = faker.Internet.Email();
            var skills = string.Join(",", faker.PickRandom(skillsPool, faker.Random.Int(2, 5)));
            var years = faker.Random.Int(0, 15);

            var jobSeeker = new JobSeeker(fullName, email)
            {
                Skills = skills,
                YearsOfExperience = years,
                DesiredJobTitle = faker.Name.JobTitle()
            };
            var result = await userManager.CreateAsync(jobSeeker, "Test@123");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(jobSeeker, "JobSeeker");
        }
    }
}