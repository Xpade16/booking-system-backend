using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("User");
        
        builder.Property(u => u.IsEmailVerified)
            .HasDefaultValue(false);
        
        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        builder.Property(u => u.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        builder.Property(u => u.RowVersion)
            .IsRowVersion();
        
        builder.HasOne(u => u.Country)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(u => u.CountryId);
    }
}