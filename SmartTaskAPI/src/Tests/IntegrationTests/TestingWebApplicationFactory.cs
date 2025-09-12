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
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests
{
    // Generic: WebApplicationFactory<Program> (Program befindet sich im Api-Projekt)
    public class TestingWebApplicationFactory : WebApplicationFactory<Api.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // 1) Add/override configuration (JWT keys + optionally other settings)
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                // Use a sufficiently long test key (base64)
                // 64 bytes base64 (here using repeated 'a' bytes then base64)
                var keyBytes = Encoding.UTF8.GetBytes(new string('a', 64));
                var base64Key = Convert.ToBase64String(keyBytes);

                var inMemorySettings = new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = base64Key,
                    ["Jwt:Issuer"] = "TestIssuer",
                    ["Jwt:Audience"] = "TestAudience",

                    // IMPORTANT: override DB connection string so the app doesn't try to connect to docker/postgres
                    // We'll actually replace the DbContext registration below, but keeping this is harmless.
                    ["ConnectionStrings:DefaultConnection"] = "DataSource=:memory:"
                };

                conf.AddInMemoryCollection(inMemorySettings);
            });

            // 2) Replace real DB registration with InMemory DB
            builder.ConfigureServices(services =>
            {
                // Remove the existing AppDbContext registration (registered in InfrastructureExtensions)
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Also remove concrete AppDbContext registrations if present
                var ctxDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
                if (ctxDescriptor != null) services.Remove(ctxDescriptor);

                // Register EF Core InMemory provider for tests
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTests_Db");
                });
                
                // Build the provider and ensure DB is created
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    // Optionally seed test data here if needed:
                    // db.TaskItems.Add(new Domain.Entities.TaskItem { ... });
                    // db.SaveChanges();
                }
            });
        }
    }
}
