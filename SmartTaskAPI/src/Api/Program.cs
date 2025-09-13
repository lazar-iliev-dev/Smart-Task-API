using System.Text;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using DotNetEnv;

// Load .env in development (optional) - safe: Env.Load() will not throw if file missing
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Konfiguration laden
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Serilog konfigurieren
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// --- JWT-Key Handling (safe) ---
string? jwtKeyRaw = builder.Configuration["Jwt:Key"]
                    ?? Environment.GetEnvironmentVariable("Jwt__Key"); // docker-compose .env style

if (string.IsNullOrWhiteSpace(jwtKeyRaw))
{
    throw new InvalidOperationException("JWT Key missing - set Jwt__Key in .env or Jwt:Key in config.");
}

// Try parse as Base64, else fall back to UTF8 bytes
byte[] jwtKeyBytes;
try
{
    jwtKeyBytes = Convert.FromBase64String(jwtKeyRaw);
    Log.Information("JWT key parsed as Base64 (length {len} bytes).", jwtKeyBytes.Length);
}
catch (FormatException)
{
    jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKeyRaw);
    Log.Information("JWT key used as UTF8 string (length {len} bytes).", jwtKeyBytes.Length);
}

// HS256 requires at least 32 bytes (256 bits)
if (jwtKeyBytes.Length < 32)
{
    throw new InvalidOperationException($"JWT key is too short ({jwtKeyBytes.Length} bytes). It must be at least 32 bytes (256 bits).");
}

// --- Authentication ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes)
        };
    });

// Infrastructure Services
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

// DEBDUG: Which DB connection is being used?
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    if (connectionString.Contains("supabase.co"))
        Log.Information("🌐 Using Supabase as database backend.");
    else
        Log.Information("🐳 Using local Docker Postgres as database backend.");
}
else
{
    Log.Warning("⚠️ No database connection string found!");
}

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App erstellen
var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

// HTTPS, Auth, Routing
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Optional: Root Route
app.MapGet("/", () => Results.Ok("✅ SmartTaskAPI running. Visit /swagger for API docs."));

        app.Run();

// For integration tests:
namespace Api
{
    public partial class Program { }
}
