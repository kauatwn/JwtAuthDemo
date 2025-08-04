using System.Security.Claims;
using JwtAuthDemo.API.DTOs.Requests;
using JwtAuthDemo.API.Entities;
using JwtAuthDemo.API.Interfaces.Repositories;
using JwtAuthDemo.API.Interfaces.Services;
using JwtAuthDemo.API.Models;
using JwtAuthDemo.API.Options;
using Microsoft.Extensions.Options;

namespace JwtAuthDemo.API.Services;

public class AuthService(IOptions<JwtOptions> options, ITokenService tokenService, IUserRepository repository)
    : IAuthService
{
    private readonly JwtOptions _options = options.Value ?? throw new ArgumentNullException(nameof(options));

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        User? user = await repository.GetByEmailAsync(request.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        ClaimsIdentity identity = tokenService.GenerateClaimsIdentity(user);

        DateTime accessExpiresAt = DateTime.UtcNow.Add(_options.AccessTokenExpiration);
        DateTime refreshExpiresAt = DateTime.UtcNow.Add(_options.RefreshTokenExpiration);

        string accessToken = tokenService.GenerateAccessToken(identity, accessExpiresAt);
        string refreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAt = refreshExpiresAt;

        await repository.SaveChangesAsync();

        return new AuthResult(accessToken, accessExpiresAt, refreshToken, refreshExpiresAt);
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        if (await repository.ExistsByEmailAsync(request.Email))
        {
            throw new InvalidOperationException("Email already registered.");
        }

        if (request.Password != request.ConfirmPassword)
        {
            throw new InvalidOperationException("Passwords do not match.");
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        User newUser = new(request.Name, request.Email, hashedPassword);
        await repository.AddAsync(newUser);

        ClaimsIdentity identity = tokenService.GenerateClaimsIdentity(newUser);

        DateTime accessExpiresAt = DateTime.UtcNow.Add(_options.AccessTokenExpiration);
        DateTime refreshExpiresAt = DateTime.UtcNow.Add(_options.RefreshTokenExpiration);

        string accessToken = tokenService.GenerateAccessToken(identity, accessExpiresAt);
        string refreshToken = tokenService.GenerateRefreshToken();

        newUser.RefreshToken = refreshToken;
        newUser.RefreshTokenExpiresAt = refreshExpiresAt;

        await repository.SaveChangesAsync();

        return new AuthResult(accessToken, accessExpiresAt, refreshToken, refreshExpiresAt);
    }

    public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
    {
        User? user = await repository.GetByRefreshTokenAsync(refreshToken);

        if (user is null || !user.RefreshTokenExpiresAt.HasValue || user.RefreshTokenExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token");
        }

        ClaimsIdentity identity = tokenService.GenerateClaimsIdentity(user);

        DateTime accessExpiresAt = DateTime.UtcNow.Add(_options.AccessTokenExpiration);
        DateTime refreshExpiresAt = DateTime.UtcNow.Add(_options.RefreshTokenExpiration);

        string newAccessToken = tokenService.GenerateAccessToken(identity, accessExpiresAt);
        string newRefreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiresAt = refreshExpiresAt;

        await repository.SaveChangesAsync();

        return new AuthResult(newAccessToken, accessExpiresAt, newRefreshToken, refreshExpiresAt);
    }
}