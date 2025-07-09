using JwtAuthDemo.API.Entities;
using System.Security.Claims;

namespace JwtAuthDemo.API.Interfaces.Services;

public interface ITokenService
{
    ClaimsIdentity GenerateClaimsIdentity(User user);
    string GenerateAccessToken(ClaimsIdentity claimsIdentity, DateTime expiresAt);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
}