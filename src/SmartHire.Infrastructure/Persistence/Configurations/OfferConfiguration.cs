using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHire.Domain.Entities;

namespace SmartHire.Infrastructure.Persistence.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("Offers");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Salary)
               .HasPrecision(18,2)
               .IsRequired();

        builder.Property(o => o.Currency)
               .IsRequired()
               .HasMaxLength(5);

        builder.Property(o => o.StartDate)
               .IsRequired();

        builder.Property(o => o.ExpirationDate)
               .IsRequired();

        builder.Property(o => o.IsAccepted)
               .IsRequired();

        builder.HasIndex(o => o.JobApplicationId)
               .IsUnique();

        builder.HasIndex(o => o.ExpirationDate);

        builder.HasOne(o => o.JobApplication)
            .WithOne(ja => ja.Offer)
            .HasForeignKey<Offer>(o => o.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}