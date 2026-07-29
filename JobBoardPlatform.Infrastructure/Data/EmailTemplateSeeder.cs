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
            new EmailTemplate("EmployerApproved", "اکانت شما تأیید شد",
                "سلام {FullName}،\nحساب کارفرمایی شرکت {CompanyName} شما توسط ادمین تأیید شد."),
            new EmailTemplate("EmployerRejected", "اکانت شما رد شد",
                "سلام {FullName}،\nمتاسفانه حساب کارفرمایی شرکت {CompanyName} شما تأیید نشد."),
            new EmailTemplate("ApplicationReviewing", "درخواست شما در حال بررسی است",
                "سلام {FullName}،\nدرخواست شما برای موقعیت {JobTitle} در حال بررسی است."),
            new EmailTemplate("ApplicationInterview", "به مصاحبه دعوت شدید",
                "سلام {FullName}،\nشما برای موقعیت {JobTitle} به مرحله‌ی مصاحبه دعوت شدید."),
            new EmailTemplate("ApplicationAccepted", "درخواست شما پذیرفته شد",
                "سلام {FullName}،\nتبریک! درخواست شما برای موقعیت {JobTitle} پذیرفته شد."),
            new EmailTemplate("ApplicationRejected", "درخواست شما رد شد",
                "سلام {FullName}،\nمتاسفانه درخواست شما برای موقعیت {JobTitle} رد شد."),
            new EmailTemplate("NewApplicationReceived", "یک درخواست جدید دریافت کردید",
                "سلام {EmployerName}،\nیک درخواست جدید برای آگهی {JobTitle} دریافت کردید.")
        };

        context.EmailTemplates.AddRange(templates);
        await context.SaveChangesAsync();
    }
}