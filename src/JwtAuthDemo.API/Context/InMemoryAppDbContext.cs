using JwtAuthDemo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDemo.API.Context;

public class InMemoryAppDbContext(DbContextOptions<InMemoryAppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Name).HasMaxLength(100);
            entity.Property(u => u.Email).HasMaxLength(256);
            entity.Property(u => u.Password).HasMaxLength(256);
            entity.Property(u => u.RefreshToken).HasMaxLength(512);
        });
        
        modelBuilder.Entity<User>().HasData(
            new User(
                name: "Admin",
                email: "admin@admin.com",
                password: "$2a$11$h6UcM3ZVSZHP35jUbL874ejPEhewHMc/CoF95/HTQ19qE.YijWVRC" // This is a hashed password for "admin1234" using SHA256
            )
            {
                Id = 1
            }
        );
    }
}