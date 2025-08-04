namespace JwtAuthDemo.API.Entities;

public class User(string name, string email, string passwordHash)
{
    public int Id { get; init; }
    public string Name { get; init; } = name;
    public string Email { get; init; } = email;
    public string PasswordHash { get; init; } = passwordHash;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpiresAt { get; set; }
}