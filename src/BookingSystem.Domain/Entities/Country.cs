namespace BookingSystem.Domain.Entities;

public class Country
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // SG, MY, TH, US
    public string Name { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}