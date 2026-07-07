using Microsoft.EntityFrameworkCore;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Identity
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Notification> Notifications => Set<Notification>();

        // Company
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Job> Jobs => Set<Job>();

        // Candidate
        public DbSet<CandidateProfile> CandidateProfiles => Set<CandidateProfile>();
        public DbSet<JobApplication> JobApplications => Set<JobApplication>();
        public DbSet<SavedJob> SavedJobs => Set<SavedJob>();

        // Skills
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<JobSkill> JobSkills => Set<JobSkill>();
        public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();

        // Recruitment
        public DbSet<Interview> Interviews => Set<Interview>();
        public DbSet<Offer> Offers => Set<Offer>();
        public DbSet<Review> Reviews => Set<Review>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
