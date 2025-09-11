using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
namespace Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JwtService> _logger;
        private readonly byte[] _keyBytes;

        public JwtService(IConfiguration config, ILogger<JwtService> logger)
        {
            _config = config;
            _logger = logger;
            var key = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(key))
            {
                _logger.LogError("JWT key missing in configuration (Jwt:Key).");
                throw new InvalidOperationException("JWT Key missing");
            }
            // Try base64 decode first, otherwise UTF8 bytes
            try
            {
                _keyBytes = Convert.FromBase64String(key);
                _logger.LogInformation("JWT key parsed as Base64 (length {len} bytes).", _keyBytes.Length);
            }
            catch (FormatException)
            {
                _keyBytes = Encoding.UTF8.GetBytes(key);
                _logger.LogInformation("JWT key used as UTF8 string (length {len} bytes).", _keyBytes.Length);
            }

            if (_keyBytes.Length < 32)
            {
                _logger.LogWarning("JWT signing key is shorter than 32 bytes ({len}). Consider using a stronger key.", _keyBytes.Length);
            }

        }

        public string GenerateToken(UserDto user)
        {
            _logger.LogInformation("Generating JWT for user {User}", user.Username);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
        };

            var key = new SymmetricSecurityKey(_keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    
    }
}