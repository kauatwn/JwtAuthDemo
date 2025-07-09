using JwtAuthDemo.API.Entities;

namespace JwtAuthDemo.API.Interfaces.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(int id);
}