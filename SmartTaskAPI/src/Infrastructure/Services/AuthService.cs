using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, ILogger<AuthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Registers a new user. Password hash is generated automatically.
    /// </summary>
    async Task<UserDto> IAuthService.RegisterAsync(string username, string password, string role)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            _logger.LogWarning("Registration attempt failed: Username {Username} already exists.", username);
            throw new InvalidOperationException("Username already exists.");
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

         _logger.LogInformation("User {Username} registered successfully.", username);

        // Mapping to DTOs
        return new UserDto
        {
            UserId = Guid.NewGuid(),
            Username = user.Username,
            Role = user.Role,
        };
    }

    /// <summary>
    /// Checks username & password. Returns the user if correct, otherwise null.
    /// </summary>
    async Task<UserDto> IAuthService.LoginAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            _logger.LogWarning("Login failed: Username {Username} not found.", username);
            return null!;
        }


        bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!valid)
        {
            _logger.LogWarning("Login failed: Invalid password for user {Username}.", username);
            return null!;
        }

        _logger.LogInformation("User {Username} logged in successfully.", username);
        
        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Role = user.Role,
        };
    }
}
