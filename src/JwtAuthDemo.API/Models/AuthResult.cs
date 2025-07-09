namespace JwtAuthDemo.API.Models;

public sealed record AuthResult(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);