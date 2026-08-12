using JobBoardPlatform.Domain.Entities.Emails;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Data;

public static class EmailTemplateSeeder
{
    public static async Task SeedDefaultTemplatesAsync(AppDbContext context)
    {
        if (await context.EmailTemplates.AnyAsync())
            return;

        var templates = new[]
        {
            new EmailTemplate("EmployerApproved", "Your account has been approved",
                "Hello {FullName},\nYour employer account for {CompanyName} has been approved by an administrator."),
            new EmailTemplate("EmployerRejected", "Your account has been rejected",
                "Hello {FullName},\nUnfortunately, your employer account for {CompanyName} was not approved."),
            new EmailTemplate("ApplicationReviewing", "Your application is under review",
                "Hello {FullName},\nYour application for {JobTitle} is now under review."),
            new EmailTemplate("ApplicationInterview", "You have been invited to an interview",
                "Hello {FullName},\nYou have been invited to interview for the {JobTitle} position."),
            new EmailTemplate("ApplicationAccepted", "Your application has been accepted",
                "Hello {FullName},\nCongratulations! Your application for {JobTitle} has been accepted."),
            new EmailTemplate("ApplicationRejected", "Your application has been rejected",
                "Hello {FullName},\nUnfortunately, your application for {JobTitle} was not successful."),
            new EmailTemplate("NewApplicationReceived", "You have received a new application",
                "Hello {EmployerName},\nYou have received a new application for {JobTitle}.")
        };

        context.EmailTemplates.AddRange(templates);
        await context.SaveChangesAsync();
    }
}
