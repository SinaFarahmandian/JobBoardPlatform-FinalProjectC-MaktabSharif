namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveResumeAsync(int jobSeekerId, Stream fileStream, string fileName);
    void DeleteResume(string storedPath);
    (Stream stream, string contentType, string fileName) GetResume(string storedPath);
}