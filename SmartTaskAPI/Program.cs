using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartTaskAPI.Services;
using SmartTaskAPI.Data;

// Serilog configuration before builder
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Connect the logger to the host
builder.Host.UseSerilog();

// Add JWT config
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddScoped<JwtService>();
builder.Services.AddControllers();

// Logger for ASP.NET Core (optional, as UseSerilog takes care of this)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Lädt appsettings.json + appsettings.{ENVIRONMENT}.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

    // Nur im Development: User Secrets laden
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Test-Log: Sicherstellen, dass das Secret ankommt
var testConn = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"[DEBUG] ConnectionString (Development): {testConn}");

// Add services to the container.
//builder.WebHost.UseUrls("http://+:80");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<MlSearchService>();

var app = builder.Build();

// Error-Handling Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
   
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Root route for status or forwarding
app.MapGet("/", (HttpContext context) =>
{
    // Either a simple message
    return Results.Ok("✅ SmartTaskApi Running! Go to /swagger for the API documentation.");

    // Or automatic forwarding to Swagger:
    // context.Response.Redirect("/swagger");
    // return Results.Empty;
});
app.Run();
