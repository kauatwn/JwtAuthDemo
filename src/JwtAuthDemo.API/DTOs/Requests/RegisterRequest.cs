namespace JwtAuthDemo.API.DTOs.Requests;

public sealed record RegisterRequest(string Name, string Email, string Password, string ConfirmPassword);