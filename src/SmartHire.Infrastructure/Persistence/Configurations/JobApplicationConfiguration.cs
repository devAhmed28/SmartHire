using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");

        builder.HasKey(ja => ja.Id);

        builder.Property(ja => ja.Status)
               .IsRequired();

        builder.Property(ja => ja.AppliedAt)
               .IsRequired();

        builder.Property(ja => ja.CoverLetter)
               .HasMaxLength(3000);

        builder.HasIndex(ja => ja.JobId);

        builder.HasIndex(ja => ja.CandidateProfileId);

        builder.HasIndex(ja => ja.Status);

        builder.HasIndex(ja => new { ja.JobId, ja.CandidateProfileId })
               .IsUnique();

        builder.HasOne(ja => ja.CandidateProfile)
           .WithMany(cp => cp.JobApplications)
           .HasForeignKey(ja => ja.CandidateProfileId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ja => ja.Job)
            .WithMany(j => j.JobApplications)
            .HasForeignKey(ja => ja.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}