using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortfolioApi.Data;
using PortfolioApi.Middleware;
using PortfolioApi.Services;
using PortfolioApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ---------- Startup diagnostics ----------
Console.WriteLine("=================================");
Console.WriteLine("Environment: " + builder.Environment.EnvironmentName);
Console.WriteLine("Content Root: " + builder.Environment.ContentRootPath);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine(
    "Database configured: " +
    !string.IsNullOrWhiteSpace(connectionString)
);

var jwtKey = builder.Configuration["Jwt:Key"];

Console.WriteLine(
    "JWT Key configured: " +
    !string.IsNullOrWhiteSpace(jwtKey)
);

Console.WriteLine("=================================");

// ---------- Configuration ----------
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection is not configured."
    );
}

if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key must be configured and at least 32 characters long."
    );
}
/*var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"];

if (string.IsNullOrWhiteSpace(allowedOrigin))
{
    throw new InvalidOperationException(
        "Cors:AllowedOrigin is not configured."
    );
}*/
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "Cors:AllowedOrigins is not configured."
    );
}
// ---------- Services ----------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(PortfolioApi.Services.Generic.CrudService<>));

// ---------- CORS ----------
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("PortfolioCorsPolicy", policy =>
    {
        policy
            .WithOrigins(allowedOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});*/
builder.Services.AddCors(options =>
{
    options.AddPolicy("PortfolioCorsPolicy", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// ---------- Authentication ----------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
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

        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// ---------- Database migration + seed ----------
// ---------- Database migration ----------
// ---------- Database migration + admin seed ----------
try
{
    Console.WriteLine("Starting database migration...");

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        // Apply pending EF Core migrations
        await db.Database.MigrateAsync();

        Console.WriteLine("Database migration completed.");

        // ---------- Seed Admin User ----------
        var adminUsername = builder.Configuration["Admin:Username"];
        var adminEmail = builder.Configuration["Admin:Email"];
        var adminPassword = builder.Configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(adminUsername) ||
            string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            Console.WriteLine(
                "Admin seed skipped: Admin credentials are not configured."
            );
        }
        else
        {
            var existingAdmin = await db.AdminUsers
                .FirstOrDefaultAsync(u => u.Username == adminUsername);

            if (existingAdmin == null)
            {
                var adminUser = new PortfolioApi.Models.AdminUser
                {
                    Username = adminUsername,
                    Email = adminEmail,
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword(adminPassword),
                    CreatedAt = DateTime.UtcNow
                };

                db.AdminUsers.Add(adminUser);

                await db.SaveChangesAsync();

                Console.WriteLine(
                    $"Admin user '{adminUsername}' created successfully."
                );
            }
            else
            {
                Console.WriteLine(
                    $"Admin user '{adminUsername}' already exists."
                );
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("DATABASE MIGRATION/SEED ERROR:");
    Console.WriteLine(ex.ToString());
}

// ---------- Middleware pipeline ----------
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("PortfolioCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();


Console.WriteLine("Portfolio API started successfully.");

app.Run();
