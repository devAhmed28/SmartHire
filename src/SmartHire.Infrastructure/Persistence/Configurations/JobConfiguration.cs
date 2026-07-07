using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(j => j.Description)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(j => j.Responsibilities)
            .HasMaxLength(3000);

        builder.Property(j => j.Requirements)
            .HasMaxLength(3000);

        builder.Property(j => j.SalaryMin)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(j => j.SalaryMax)
            .HasColumnType("decimal(18,2)");

        builder.Property(j => j.Currency)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(j => j.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(j => j.ExpirationDate)
            .IsRequired();

        builder.Property(j => j.Vacancies)
            .IsRequired();

        builder.HasIndex(j => j.CompanyId);

        builder.HasIndex(j => j.JobStatus);

        builder.HasIndex(j => j.ExpirationDate);

        builder.HasOne(j => j.Company)
            .WithMany(c => c.Jobs)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}