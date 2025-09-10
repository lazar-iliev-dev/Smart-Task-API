using System.Text;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog konfigurieren
Log.Logger = new LoggerConfiguration()
    //.ReadFrom.Configuration(builder.Configuration) // optional, liest aus appsettings.json
    //.MinimumLevel.Debug() 
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Infrastructure Services
builder.Services.AddInfrastructure(builder.Configuration);

// Konfiguration laden
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

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

// JWT Authentication
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key missing"))
            )
        };
    });
// Jwt Key Check 
var key = builder.Configuration["Jwt:Key"];
Log.Information("Jwt key present: {present}", !string.IsNullOrEmpty(key));

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//new---
builder.Services.AddInfrastructure(builder.Configuration);

//  Host-Logging wieder aktivieren
// builder.Logging.ClearProviders();      // entferne default Logger
// builder.Logging.AddConsole();          // füge Konsolen-Logger wieder hinzu
// builder.Logging.AddDebug();            // optional Debug-Logger

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


// 👇 For integration tests:
namespace Api
{
    public partial class Program { }
}
