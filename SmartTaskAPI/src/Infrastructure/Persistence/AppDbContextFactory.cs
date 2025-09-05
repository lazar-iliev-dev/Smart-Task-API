using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 1) ENV first (most reliable for CLI/CI)
        var envConn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (!string.IsNullOrWhiteSpace(envConn))
        {
            Console.WriteLine("[AppDbContextFactory] Using Connection from ENV");
            var optsEnv = new DbContextOptionsBuilder<AppDbContext>();
            optsEnv.UseNpgsql(envConn);
            return new AppDbContext(optsEnv.Options);
        }

        // 2) Try to find appsettings.json in several likely base paths
        string[] candidates =
        {
            Directory.GetCurrentDirectory(),                         // usually the working dir
            AppContext.BaseDirectory,                                // assembly base dir
            Path.Combine(Directory.GetCurrentDirectory(), "../Api"), // sibling Api (relative)
            Path.Combine(Directory.GetCurrentDirectory(), "Api"),    // sometimes nested
        };

        IConfiguration? configuration = null;
        foreach (var basePath in candidates)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Path.GetFullPath(basePath))
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.Development.json", optional: true)
                    .AddEnvironmentVariables();

                // add user secrets if available (safe to try/catch)
                try
                {
                    builder.AddUserSecrets<AppDbContextFactory>(optional: true);
                }
                catch { /* ignore if not available */ }

                var config = builder.Build();
                var c = config.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrWhiteSpace(c))
                {
                    configuration = config;
                    Console.WriteLine($"[AppDbContextFactory] Found connection in basePath: {Path.GetFullPath(basePath)}");
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AppDbContextFactory] config try failed for {basePath}: {ex.Message}");
            }
        }

        var conn = configuration?.GetConnectionString("DefaultConnection")
                   ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        if (string.IsNullOrWhiteSpace(conn))
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Set via UserSecrets, appsettings.json or environment variable (ConnectionStrings__DefaultConnection).");

        var options = new DbContextOptionsBuilder<AppDbContext>();
        options.UseNpgsql(conn);

        Console.WriteLine($"[AppDbContextFactory] Using Connection: {(conn.Length > 80 ? conn[..80] + "..." : conn)}");
        return new AppDbContext(options.Options);
    }
}
