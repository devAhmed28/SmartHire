using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
{
    public void Configure(EntityTypeBuilder<JobSkill> builder)
    {
        builder.ToTable("JobSkills");

        builder.HasKey(js => js.Id);

        builder.Property(js => js.IsRequired)
               .IsRequired();

        builder.HasIndex(js => js.JobId);

        builder.HasIndex(js => js.SkillId);

        builder.HasOne(js => js.Job)
            .WithMany(j => j.JobSkills)
            .HasForeignKey(js => js.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(js => js.Skill)
            .WithMany(j => j.JobSkills)
            .HasForeignKey(js => js.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}