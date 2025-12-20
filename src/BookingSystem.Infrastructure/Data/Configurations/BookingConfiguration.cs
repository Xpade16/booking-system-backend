using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.CreditsUsed)
            .IsRequired();
        
        builder.Property(b => b.BookedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        builder.Property(b => b.CheckedInAt)
            .IsRequired(false);
        
        builder.Property(b => b.IsCancelled)
            .HasDefaultValue(false);
        
        builder.Property(b => b.CancelledAt)
            .IsRequired(false);
        
        builder.Property(b => b.IsRefunded)
            .HasDefaultValue(false);
        
        builder.Property(b => b.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("Confirmed");
        
        builder.Property(b => b.RowVersion)
            .IsRowVersion();
        
        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(b => b.ClassSchedule)
            .WithMany(cs => cs.Bookings)
            .HasForeignKey(b => b.ClassScheduleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(b => b.UserPackage)
            .WithMany(up => up.Bookings)
            .HasForeignKey(b => b.UserPackageId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(b => new { b.UserId, b.ClassScheduleId });
        builder.HasIndex(b => new { b.ClassScheduleId, b.Status });
        builder.HasIndex(b => new { b.UserId, b.BookedAt });
    }
}