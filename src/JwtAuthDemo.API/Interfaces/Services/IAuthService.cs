using JwtAuthDemo.API.DTOs.Requests;
using JwtAuthDemo.API.Models;

namespace JwtAuthDemo.API.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequest request);
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task<AuthResult> RefreshTokenAsync(string refreshToken);
}