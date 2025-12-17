using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingSystem.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAdminUserAsync(ApplicationDbContext context, ILogger logger)
    {
        // Check if admin already exists
        var adminExists = await context.Users.AnyAsync(u => u.Email == "admin@bookingsystem.com");
        
        if (!adminExists)
        {
            var sgCountry = await context.Countries.FirstOrDefaultAsync(c => c.Code == "SG");
            
            if (sgCountry == null)
            {
                logger.LogWarning("Cannot create admin user: Singapore country not found");
                return;
            }

            var adminUser = new User
            {
                Email = "admin@bookingsystem.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!@#"),
                FirstName = "System",
                LastName = "Admin",
                CountryId = sgCountry.Id,
                IsEmailVerified = true,
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            logger.LogInformation("Admin user created: admin@bookingsystem.com / Admin123!@#");
        }
    }
}