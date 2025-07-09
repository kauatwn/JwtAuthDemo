namespace JwtAuthDemo.API.Options;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";

    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;

    public TimeSpan AccessTokenExpiration { get; init; }
    public TimeSpan RefreshTokenExpiration { get; init; }
}