using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Api;

namespace IntegrationTests
{
    public class AuthEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthEndpointsTests(WebApplicationFactory<Program> factory)
        {
            //Create client from test server
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_And_Login_Should_Return_JwtToken()
        {
            // Arrange: Test user data
            var testUser = new
            {
                Username = "integrationtestuser",
                Password = "Pasword123!",
                Role = "User"
            };

            // Act 1: Registrierung
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", testUser);

            // Assert Registrierung
            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

            // Act 2: Login
            var loginData = new
            {
                Username = testUser.Username,
                Password = testUser.Password
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginData);

            // Assert Login
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var jwt = await loginResponse.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(jwt));
            Assert.Contains("ey", jwt); // simple check whether JWT is returned
        }
    }
}
