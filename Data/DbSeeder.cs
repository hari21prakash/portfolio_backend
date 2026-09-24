using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data;

// Ensures the database has exactly one admin account and the singleton
// Profile / SiteSettings rows so the API never 404s on a fresh database.
public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db, IConfiguration config, ILogger logger)
    {
        await db.Database.MigrateAsync();

        if (!await db.AdminUsers.AnyAsync())
        {
            var seedUsername = config["Seed:AdminUsername"] ?? "admin";
            var seedEmail = config["Seed:AdminEmail"] ?? "admin@example.com";
            var seedPassword = config["Seed:AdminPassword"];

            if (string.IsNullOrWhiteSpace(seedPassword))
            {
                logger.LogWarning(
                    "No Seed:AdminPassword configured — skipping admin user creation. " +
                    "Set Seed__AdminPassword as an environment variable and restart to create the initial admin account.");
            }
            else
            {
                db.AdminUsers.Add(new AdminUser
                {
                    Username = seedUsername,
                    Email = seedEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(seedPassword),
                    CreatedAt = DateTime.UtcNow
                });
                logger.LogInformation("Seeded initial admin user '{Username}'.", seedUsername);
            }
        }

        if (!await db.Profiles.AnyAsync())
        {
            db.Profiles.Add(new Profile
            {
                FullName = "Your Name",
                Title = "Full-Stack Developer",
                Bio = "Update this bio from the Admin CMS.",
                ShortSummary = "Building things for the web.",
                Email = "you@example.com",
                UpdatedAt = DateTime.UtcNow
            });
        }

        if (!await db.SiteSettings.AnyAsync())
        {
            db.SiteSettings.Add(new SiteSetting
            {
                SiteTitle = "My Portfolio",
                SiteDescription = "Personal developer portfolio.",
                DefaultTheme = "light",
                UpdatedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();
    }
}
