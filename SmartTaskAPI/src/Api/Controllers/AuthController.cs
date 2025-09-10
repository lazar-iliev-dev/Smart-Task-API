using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// User-Registration
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterAsync(request.Username, request.Password, request.Role);

                _logger.LogInformation("User {Username} registered successfully.", request.Username);

                if (string.IsNullOrWhiteSpace(request.Username) ||
                 string.IsNullOrWhiteSpace(request.Password) ||
                 string.IsNullOrWhiteSpace(request.Role))
                {
                    return BadRequest(new { message = "Username, Password and Role are required." });
                }

                return Ok(user);

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Registration failed for {Username}: {Message}", request.Username, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Login & JWT Token Ausgabe
        /// </summary>
        //[HttpPost("login")]
        // public async Task<IActionResult> Login([FromBody] LoginRequest request)
        // {
        //     var user = await _authService.LoginAsync(request.Username, request.Password);
        //     if (user == null)
        //     {
        //         _logger.LogWarning("Login failed for {Username}", request.Username);
        //         return Unauthorized(new { message = "Invalid username or password" });
        //     }

        //     // Token generieren
        //     var domainUser = _jwtService.GenerateToken(new UserDto
        //     {
        //         UserId = user.UserId,
        //         Username = user.Username,
        //         Role = user.Role
        //     });

        //     _logger.LogInformation("User {Username} logged in successfully.", request.Username);
        //     return Ok(new { domainUser });
        // }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var userDto = await _authService.LoginAsync(request.Username, request.Password);

            if (userDto == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Domain-User für Token bauen
            var domainUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = userDto.Username,
                Role = userDto.Role
            };

            var token = _jwtService.GenerateToken(new UserDto
            {
                UserId = domainUser.UserId,
                Username = userDto.Username,
                Role = domainUser.Role
            });

            return Ok(new { token });
        }
        catch (Exception ex)
        {
            // Wichtig: Logging
            Console.WriteLine($"❌ Login failed: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    }
    
    /// <summary>
   /// DTO for registration   
   /// </summary>
    public record RegisterRequest(string Username, string Password, string Role);

    /// <summary>
    /// DTO for login    
    /// </summary>
    public record LoginRequest(string Username, string Password);
}
