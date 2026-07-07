using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.ToTable("Interviews");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.InterviewDate)
               .IsRequired();

        builder.Property(i => i.Type)
               .IsRequired();

        builder.Property(i => i.Status)
               .IsRequired();

        builder.Property(i => i.MeetingLink)
               .HasMaxLength(500);

        builder.Property(i => i.Notes)
               .HasMaxLength(2000);

        builder.HasIndex(i => i.JobApplicationId);

        builder.HasIndex(i => i.InterviewDate);

        builder.HasIndex(i => i.Status);

        builder.HasOne(i => i.JobApplication)
            .WithMany(ja => ja.Interviews)
            .HasForeignKey(i => i.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}