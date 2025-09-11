using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.UnitTests
{
    // Fake JWT Service für Unit-Tests
    public class FakeJwtService : IJwtService
    {
        public string GenerateToken(UserDto user)
        {
            return "fake-jwt-token-for-" + user.Username;
        }
    }

    public class AuthServiceTests : IDisposable
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _context;
        private readonly FakeJwtService _fakeJwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthServiceTests()
        {
            // readonly Felder initialisieren
            _fakeJwtService = new FakeJwtService();

            // Logger auf Null setzen (optional: Mock ILogger verwenden)
            _logger = new LoggerFactory().CreateLogger<AuthService>();

            // InMemory DB statt echter Postgres
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // jede Testinstanz eigene DB
                .Options;

            _context = new AppDbContext(options);

            // AuthService mit InMemory DB und Fake JWT Service
            _authService = new AuthService(_context, _logger);
        }

        [Fact]
        public async Task RegisterAsync_Should_RegisterUserSuccessfully()
        {
            // Arrange
            string username = "unitTestUser";
            string password = "Password123!";
            string role = "User";

            // Act
            var result = await ((IAuthService)_authService).RegisterAsync(username, password, role);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
            Assert.Equal(role, result.Role);

            // DB enthält nun einen User
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            Assert.NotNull(userInDb);
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnUserDto_WhenCredentialsAreValid()
        {
            // Arrange
            string username = "loginUser";
            string password = "Password123!";
            string role = "User";

            // User registrieren
            await ((IAuthService)_authService).RegisterAsync(username, password, role);

            // Act
            var loginResult = await ((IAuthService)_authService).LoginAsync(username, password);

            // Assert
            Assert.NotNull(loginResult);
            Assert.Equal(username, loginResult.Username);
            Assert.Equal(role, loginResult.Role);
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnNull_WhenPasswordIsInvalid()
        {
            // Arrange
            string username = "wrongPasswordUser";
            string password = "Password123!";
            string role = "User";

            await ((IAuthService)_authService).RegisterAsync(username, password, role);

            // Act
            var loginResult = await ((IAuthService)_authService).LoginAsync(username, "wrongPassword!");

            // Assert
            Assert.Null(loginResult);
        }

        [Fact]
        public async Task RegisterAsync_Should_Throw_WhenUsernameExists()
        {
            // Arrange
            string username = "duplicateUser";
            string password = "Password123!";
            string role = "User";

            await ((IAuthService)_authService).RegisterAsync(username, password, role);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                ((IAuthService)_authService).RegisterAsync(username, password, role)
            );
        }

        public void Dispose()
        {
            // InMemory DB löschen nach jedem Test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
