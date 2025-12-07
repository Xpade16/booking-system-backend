using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Infrastructure.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.HasIndex(c => c.Code)
            .IsUnique();
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.TimeZone)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);
        
        builder.HasIndex(c => c.IsActive);
        
        // Seed data
        builder.HasData(
            new Country { Id = 1, Code = "SG", Name = "Singapore", TimeZone = "Asia/Singapore", IsActive = true },
            new Country { Id = 2, Code = "MY", Name = "Malaysia", TimeZone = "Asia/Kuala_Lumpur", IsActive = true },
            new Country { Id = 3, Code = "TH", Name = "Thailand", TimeZone = "Asia/Bangkok", IsActive = true },
            new Country { Id = 4, Code = "US", Name = "United States", TimeZone = "America/New_York", IsActive = true }
        );
    }
}