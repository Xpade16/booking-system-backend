using Microsoft.EntityFrameworkCore;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Country> Countries { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}