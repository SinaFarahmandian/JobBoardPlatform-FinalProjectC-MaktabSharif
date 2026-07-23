using JobBoardPlatform.Domain.Entities.JobSeekers;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJobSeekerRepository
{
    Task<JobSeeker?> GetByIdAsync(int id);
    Task UpdateAsync(JobSeeker jobSeeker);
}