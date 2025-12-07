namespace BookingSystem.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public bool IsEmailVerified { get; set; }
    public string Role { get; set; } = "User"; // User, Admin
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    
    // Navigation properties
    public Country Country { get; set; } = null!;
}