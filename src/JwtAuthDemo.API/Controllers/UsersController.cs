using JwtAuthDemo.API.DTOs.Responses;
using JwtAuthDemo.API.Entities;
using JwtAuthDemo.API.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthDemo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileResponse>> GetUserProfile(IUserService userService)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("User ID not found in claims.");
        }

        User? user = await userService.GetByIdAsync(userId);

        if (user is null)
        {
            return NotFound("User not found.");
        }

        UserProfileResponse response = new(user.Name, user.Email);
        return Ok(response);
    }
}