namespace JwtAuthDemo.API.DTOs.Responses;

public sealed record AuthSessionResponse(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);