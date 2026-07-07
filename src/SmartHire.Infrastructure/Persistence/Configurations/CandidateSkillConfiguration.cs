using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class CandidateSkillConfiguration : IEntityTypeConfiguration<CandidateSkill>
{
    public void Configure(EntityTypeBuilder<CandidateSkill> builder)
    {
        builder.ToTable("CandidateSkills");

        builder.HasKey(cs => cs.Id);

        builder.Property(cs => cs.YearsOfExperience)
               .IsRequired();

        builder.HasIndex(cs => cs.CandidateProfileId);

        builder.HasIndex(cs => cs.SkillId);

        builder.HasIndex(cs => new { cs.CandidateProfileId, cs.SkillId })
               .IsUnique();

        builder.HasOne(cs => cs.CandidateProfile)
            .WithMany(cp => cp.CandidateSkills)
            .HasForeignKey(cs => cs.CandidateProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cs => cs.Skill)
            .WithMany(s => s.CandidateSkills)
            .HasForeignKey(cs => cs.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}