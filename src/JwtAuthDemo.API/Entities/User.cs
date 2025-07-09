namespace JwtAuthDemo.API.Entities;

public class User(string name, string email, string password)
{
    public int Id { get; init; }
    public string Name { get; init; } = name;
    public string Email { get; init; } = email;
    public string Password { get; init; } = password;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAt { get; set; } = DateTime.MinValue;
}