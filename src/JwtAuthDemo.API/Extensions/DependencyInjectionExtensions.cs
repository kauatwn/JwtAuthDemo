using JwtAuthDemo.API.Context;
using JwtAuthDemo.API.Interfaces.Repositories;
using JwtAuthDemo.API.Interfaces.Services;
using JwtAuthDemo.API.Options;
using JwtAuthDemo.API.Repositories;
using JwtAuthDemo.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtAuthDemo.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        AddOptionsConfiguration(services, configuration);
        AddJwtAuthentication(services, configuration);
        AddInMemoryDbContext(services);
        AddRepositories(services);
        AddApplicationServices(services);
    }

    private static void AddInMemoryDbContext(IServiceCollection services)
    {
        services.AddDbContext<InMemoryAppDbContext>(options => options.UseInMemoryDatabase("JwtAuthDemoDb"));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddOptionsConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
    }

    private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT configuration section is missing");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}