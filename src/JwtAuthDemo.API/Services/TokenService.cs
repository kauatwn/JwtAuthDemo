using JwtAuthDemo.API.Entities;
using JwtAuthDemo.API.Interfaces.Services;
using JwtAuthDemo.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthDemo.API.Services;

public class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    private readonly JwtOptions _options = options.Value ?? throw new ArgumentNullException(nameof(options));

    public ClaimsIdentity GenerateClaimsIdentity(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        return new ClaimsIdentity(claims);
    }

    public string GenerateAccessToken(ClaimsIdentity claimsIdentity, DateTime expiresAt)
    {
        JwtSecurityTokenHandler handler = new();

        SigningCredentials credentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256Signature);


        SecurityTokenDescriptor descriptor = new()
        {
            Subject = claimsIdentity,
            Expires = expiresAt,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = credentials
        };

        SecurityToken token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
    {
        TokenValidationParameters parameters = new()
        {
            ValidateAudience = true,
            ValidAudience = _options.Audience,
            ValidateIssuer = true,
            ValidIssuer = _options.Issuer,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };

        JwtSecurityTokenHandler handler = new();

        ClaimsPrincipal principal = handler.ValidateToken(expiredToken, parameters, out SecurityToken? token);

        if (token is not JwtSecurityToken jwt || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}