using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Infrastructure.Data.Configurations;

public class ClassScheduleConfiguration : IEntityTypeConfiguration<ClassSchedule>
{
    public void Configure(EntityTypeBuilder<ClassSchedule> builder)
    {
        builder.ToTable("ClassSchedules");
        
        builder.HasKey(cs => cs.Id);
        
        builder.Property(cs => cs.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(cs => cs.Description)
            .HasMaxLength(1000);
        
        builder.Property(cs => cs.StartTime)
            .IsRequired();
        
        builder.Property(cs => cs.EndTime)
            .IsRequired();
        
        builder.Property(cs => cs.Capacity)
            .IsRequired();
        
        builder.Property(cs => cs.AvailableSlots)
            .IsRequired();
        
        builder.Property(cs => cs.RequiredCredits)
            .IsRequired()
            .HasDefaultValue(1);
        
        builder.Property(cs => cs.IsActive)
            .HasDefaultValue(true);
        
        builder.Property(cs => cs.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        builder.Property(cs => cs.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        builder.Property(cs => cs.RowVersion)
            .IsRowVersion();
        
        builder.HasOne(cs => cs.Country)
            .WithMany(c => c.ClassSchedules)
            .HasForeignKey(cs => cs.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(cs => new { cs.CountryId, cs.StartTime, cs.IsActive });
        builder.HasIndex(cs => cs.StartTime);
        
        // Seed sample classes for Singapore
        SeedSampleClasses(builder);
    }

    private void SeedSampleClasses(EntityTypeBuilder<ClassSchedule> builder)
    {
        var now = DateTime.UtcNow;
        var baseDate = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0, DateTimeKind.Utc);
        
        var classes = new List<ClassSchedule>();
        
        for (int i = 1; i <= 14; i++)
        {
            var classDate = baseDate.AddDays(i);
            
            // Morning Yoga Class
            classes.Add(new ClassSchedule
            {
                Id = i * 2 - 1,
                Title = $"Morning Yoga - Day {i}",
                Description = "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.",
                CountryId = 1, // Singapore
                StartTime = classDate.AddHours(0), // 10 AM
                EndTime = classDate.AddHours(1), // 11 AM
                Capacity = 15,
                AvailableSlots = 15,
                RequiredCredits = 1,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
            
            // Evening HIIT Class
            classes.Add(new ClassSchedule
            {
                Id = i * 2,
                Title = $"Evening HIIT - Day {i}",
                Description = "High-intensity interval training to boost your metabolism and build strength. Intermediate level.",
                CountryId = 1, // Singapore
                StartTime = classDate.AddHours(8), // 6 PM
                EndTime = classDate.AddHours(9), // 7 PM
                Capacity = 20,
                AvailableSlots = 20,
                RequiredCredits = 1,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
        }
        
        builder.HasData(classes);
    }
}