using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Infrastructure.Data.Configurations;

public class UserPackageConfiguration : IEntityTypeConfiguration<UserPackage>
{
    public void Configure(EntityTypeBuilder<UserPackage> builder)
    {
        builder.ToTable("UserPackages");
        
        builder.HasKey(up => up.Id);
        
        builder.Property(up => up.RemainingCredits)
            .IsRequired();
        
        builder.Property(up => up.PurchasedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        builder.Property(up => up.ExpiresAt)
            .IsRequired();
        
        builder.Property(up => up.IsExpired)
            .HasDefaultValue(false);
        
        builder.Property(up => up.TransactionId)
            .HasMaxLength(100);
        
        builder.Property(up => up.RowVersion)
            .IsRowVersion();
        
        builder.HasOne(up => up.User)
            .WithMany(u => u.UserPackages)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(up => up.Package)
            .WithMany(p => p.UserPackages)
            .HasForeignKey(up => up.PackageId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(up => new { up.UserId, up.IsExpired, up.ExpiresAt });
        builder.HasIndex(up => new { up.ExpiresAt, up.IsExpired });
    }
}