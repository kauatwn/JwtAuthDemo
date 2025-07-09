using JwtAuthDemo.API.Context;
using JwtAuthDemo.API.Entities;
using JwtAuthDemo.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDemo.API.Repositories;

public class UserRepository(InMemoryAppDbContext context) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}