using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class TestingWebApplicationFactory : WebApplicationFactory<Api.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext registration (Postgres)
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                var ctxDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
                if (ctxDescriptor != null)
                    services.Remove(ctxDescriptor);

                // Register InMemory provider
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTestsDb");
                });

                // Override JWT settings with a valid test key
                var key = Convert.ToBase64String(Encoding.UTF8.GetBytes(new string('a', 32)));

                var inMemorySettings = new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = key,
                    ["Jwt:Issuer"] = "TestIssuer",
                    ["Jwt:Audience"] = "TestAudience"
                };

                services.AddSingleton<IConfiguration>(sp =>
                {
                    var configurationBuilder = new ConfigurationBuilder();
                    configurationBuilder.AddInMemoryCollection(inMemorySettings);
                    return configurationBuilder.Build();
                });

                // Ensure DB schema exists
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureDeleted();   // Reset for clean tests
                db.Database.EnsureCreated();
            });
        }
    }
}
