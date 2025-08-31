using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DB-Kontext (Postgres als Beispiel)
        var conn = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseNpgsql(conn);
        });
        
        // Infrastruktur-Services
        services.AddScoped<IHealthCheckService, HealthCheckService>();


        return services;
    }
}