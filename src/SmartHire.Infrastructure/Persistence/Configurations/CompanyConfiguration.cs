using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configuration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .HasMaxLength(2000);

            builder.Property(c => c.Industry)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.WebsiteUrl)
                .HasMaxLength(500);

            builder.Property(c => c.LogoUrl)
                .HasMaxLength(500);

            builder.Property(c => c.Address)
                .HasMaxLength(300);

            builder.Property(c => c.City)
                .HasMaxLength(100);

            builder.Property(c => c.Country)
                .HasMaxLength(100);

            builder.HasIndex(c => c.UserId)
                .IsUnique();

            builder.HasOne(c => c.User)
                .WithOne(u => u.Company)
                .HasForeignKey<Company>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
