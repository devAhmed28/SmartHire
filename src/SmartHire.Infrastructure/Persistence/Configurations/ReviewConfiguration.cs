using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Rating)
               .IsRequired();

        builder.Property(r => r.Comment)
               .HasMaxLength(2000);

        builder.HasIndex(r => r.CompanyId);

        builder.HasIndex(r => r.UserId);

        builder.HasIndex(r => new { r.CompanyId, r.UserId })
               .IsUnique();

        builder.HasOne(r => r.Company)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}