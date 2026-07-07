using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class CandidateProfileConfiguration : IEntityTypeConfiguration<CandidateProfile>
{
    public void Configure(EntityTypeBuilder<CandidateProfile> builder)
    {
        builder.ToTable("CandidateProfiles");

        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.Bio)
            .HasMaxLength(2000);

        builder.Property(cp => cp.CVUrl)
            .HasMaxLength(500);

        builder.Property(cp => cp.LinkedInUrl)
            .HasMaxLength(500);

        builder.Property(cp => cp.GitHubUrl)
            .HasMaxLength(500);

        builder.Property(cp => cp.PortfolioUrl)
            .HasMaxLength(500);

        builder.Property(cp => cp.YearsOfExperience);

        builder.HasIndex(cp => cp.UserId)
            .IsUnique();

        builder.Property(cp => cp.ExpectedSalary)
       .HasPrecision(18, 2);

        builder.HasOne(cp => cp.User)
            .WithOne(u => u.CandidateProfile)
            .HasForeignKey<CandidateProfile>(cp => cp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}