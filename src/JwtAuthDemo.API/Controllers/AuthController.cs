using JwtAuthDemo.API.DTOs.Requests;
using JwtAuthDemo.API.DTOs.Responses;
using JwtAuthDemo.API.Interfaces.Services;
using JwtAuthDemo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace JwtAuthDemo.API.Controllers;

[EnableRateLimiting("auth")]
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType<AuthSessionResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthSessionResponse>> Login(LoginRequest request)
    {
        try
        {
            AuthResult result = await authService.LoginAsync(request);

            return Ok(new AuthSessionResponse(result.AccessToken, result.AccessTokenExpiresAt, result.RefreshToken,
                result.RefreshTokenExpiresAt));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }
    }

    [HttpPost("register")]
    [ProducesResponseType<AuthSessionResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthSessionResponse>> Register(RegisterRequest request)
    {
        try
        {
            AuthResult result = await authService.RegisterAsync(request);

            return Ok(new AuthSessionResponse(result.AccessToken, result.AccessTokenExpiresAt, result.RefreshToken,
                result.RefreshTokenExpiresAt));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType<AuthSessionResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthSessionResponse>> RefreshToken(RefreshTokenRequest request)
    {
        try
        {
            AuthResult result = await authService.RefreshTokenAsync(request.RefreshToken);

            return Ok(new AuthSessionResponse(result.AccessToken, result.AccessTokenExpiresAt, result.RefreshToken,
                result.RefreshTokenExpiresAt));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }
    }
}