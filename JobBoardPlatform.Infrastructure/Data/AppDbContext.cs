using JobBoardPlatform.Domain.Entities;
using JobBoardPlatform.Domain.Entities.Admins;
using JobBoardPlatform.Domain.Entities.Employers;
using JobBoardPlatform.Domain.Entities.JobApplications;
using JobBoardPlatform.Domain.Entities.JobPostings;
using JobBoardPlatform.Domain.Entities.JobSeekers;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<JobSeeker> JobSeekers => Set<JobSeeker>();
    public DbSet<Employer> Employers => Set<Employer>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<JobPosting> JobPostings => Set<JobPosting>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<JobSeeker>("JobSeeker")
            .HasValue<Employer>("Employer")
            .HasValue<Admin>("Admin");

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<JobPosting>().HasQueryFilter(jp => !jp.IsDeleted);
        modelBuilder.Entity<JobApplication>().HasQueryFilter(ja => !ja.IsDeleted);

        modelBuilder.Entity<JobPosting>()
            .HasOne(jp => jp.Employer)
            .WithMany(e => e.JobPostings)
            .HasForeignKey(jp => jp.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobPosting)
            .WithMany(jp => jp.Applications)
            .HasForeignKey(ja => ja.JobPostingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobSeeker)
            .WithMany(js => js.Applications)
            .HasForeignKey(ja => ja.JobSeekerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}