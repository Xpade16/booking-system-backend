using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Infrastructure.Data.Configurations;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.ToTable("Packages");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.Credits)
            .IsRequired();
        
        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)");
        
        builder.Property(p => p.ValidityDays)
            .IsRequired();
        
        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);
        
        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        builder.HasOne(p => p.Country)
            .WithMany(c => c.Packages)
            .HasForeignKey(p => p.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(p => new { p.CountryId, p.IsActive });
        
        // Seed data for Singapore
        builder.HasData(
            new Package 
            { 
                Id = 1,
                Name = "Trial Pack", 
                Credits = 5, 
                Price = 50.00m, 
                CountryId = 1, // Singapore
                ValidityDays = 30,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Package 
            { 
                Id = 2,
                Name = "Standard Pack", 
                Credits = 10, 
                Price = 90.00m, 
                CountryId = 1,
                ValidityDays = 60,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Package 
            { 
                Id = 3,
                Name = "Premium Pack", 
                Credits = 20, 
                Price = 160.00m, 
                CountryId = 1,
                ValidityDays = 90,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Package 
            { 
                Id = 4,
                Name = "Ultimate Pack", 
                Credits = 50, 
                Price = 350.00m, 
                CountryId = 1,
                ValidityDays = 180,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}