using JwtAuthDemo.API.Entities;
using JwtAuthDemo.API.Interfaces.Repositories;
using JwtAuthDemo.API.Interfaces.Services;

namespace JwtAuthDemo.API.Services;

public class UserService(IUserRepository repository) : IUserService
{
    public async Task<User?> GetByIdAsync(int id)
    {
        return await repository.GetByIdAsync(id);
    }
}