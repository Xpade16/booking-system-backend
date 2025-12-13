namespace BookingSystem.Domain.Entities;

public class Package
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Credits { get; set; }
    public decimal Price { get; set; }
    public int CountryId { get; set; }
    public int ValidityDays { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Country Country { get; set; } = null!;
    public ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
}