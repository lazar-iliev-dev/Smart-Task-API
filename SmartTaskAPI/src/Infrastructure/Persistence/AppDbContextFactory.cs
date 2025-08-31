using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Environment auf Development setzen, damit optional JSON geladen wird
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true) // optional!
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true) // optional!
            .AddEnvironmentVariables(); // Environment Variables haben Vorrang

        var configuration = builder.Build();

        var conn = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(conn))
        {
            // versuche direkt aus EnvVariable
            conn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        }

        if (string.IsNullOrWhiteSpace(conn))
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found. Set via UserSecrets, appsettings.json or Environment Variable.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(conn);

        Console.WriteLine($"[AppDbContextFactory] Using Connection: {conn}");

        return new AppDbContext(optionsBuilder.Options);
    }
}
