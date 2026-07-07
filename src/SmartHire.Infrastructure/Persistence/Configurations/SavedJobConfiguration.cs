using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
{
    public void Configure(EntityTypeBuilder<SavedJob> builder)
    {
        builder.ToTable("SavedJobs");

        builder.HasKey(sj => sj.Id);

        builder.HasIndex(sj => sj.CandidateProfileId);

        builder.HasIndex(sj => sj.JobId);

        builder.HasIndex(sj => new { sj.CandidateProfileId, sj.JobId })
               .IsUnique();

        builder.HasOne(sj => sj.CandidateProfile)
            .WithMany(cp => cp.SavedJobs)
            .HasForeignKey(sj => sj.CandidateProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sj => sj.Job)
            .WithMany(j => j.SavedJobs)
            .HasForeignKey(sj => sj.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}