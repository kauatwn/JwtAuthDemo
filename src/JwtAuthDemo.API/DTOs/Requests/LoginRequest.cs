﻿namespace JwtAuthDemo.API.DTOs.Requests;

public sealed record LoginRequest(string Email, string Password);