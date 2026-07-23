using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace JobBoardPlatform.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _rootPath;

    public LocalFileStorageService(IConfiguration configuration, IWebHostEnvironment env)
    {
        var relativePath = configuration["FileStorage:ResumeRootPath"] ?? "App_Data/Resumes";
        _rootPath = Path.Combine(env.ContentRootPath, relativePath);
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<string> SaveResumeAsync(int jobSeekerId, Stream fileStream, string fileName)
    {
        var userFolder = Path.Combine(_rootPath, jobSeekerId.ToString());
        Directory.CreateDirectory(userFolder);

        var storedFileName = $"{Guid.NewGuid()}.pdf";
        var fullPath = Path.Combine(userFolder, storedFileName);

        await using var output = File.Create(fullPath);
        await fileStream.CopyToAsync(output);

        return Path.Combine(jobSeekerId.ToString(), storedFileName);
    }

    public void DeleteResume(string storedPath)
    {
        var fullPath = Path.Combine(_rootPath, storedPath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public (Stream stream, string contentType, string fileName) GetResume(string storedPath)
    {
        var fullPath = Path.Combine(_rootPath, storedPath);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("فایل رزومه پیدا نشد");

        return (File.OpenRead(fullPath), "application/pdf", Path.GetFileName(fullPath));
    }
}