using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<SkillCategory> SkillCategories => Set<SkillCategory>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTechnology> ProjectTechnologies => Set<ProjectTechnology>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Certification> Certifications => Set<Certification>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<SocialLink> SocialLinks => Set<SocialLink>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------- AdminUser ----------
        modelBuilder.Entity<AdminUser>(e =>
        {
            e.HasIndex(x => x.Username).IsUnique();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Username).IsRequired().HasMaxLength(100);
            e.Property(x => x.Email).IsRequired().HasMaxLength(255);
            e.Property(x => x.PasswordHash).IsRequired();
        });

        // ---------- Profile ----------
        modelBuilder.Entity<Profile>(e =>
        {
            e.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            e.Property(x => x.Email).IsRequired().HasMaxLength(255);
        });

        // ---------- SkillCategory ----------
        modelBuilder.Entity<SkillCategory>(e =>
        {
            e.HasIndex(x => x.Name).IsUnique();
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        // ---------- Skill ----------
        modelBuilder.Entity<Skill>(e =>
        {
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasOne(x => x.SkillCategory)
                .WithMany(c => c.Skills)
                .HasForeignKey(x => x.SkillCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- Technology ----------
        modelBuilder.Entity<Technology>(e =>
        {
            e.HasIndex(x => x.Name).IsUnique();
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        // ---------- Project ----------
        modelBuilder.Entity<Project>(e =>
        {
            e.HasIndex(x => x.Slug).IsUnique();
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Slug).IsRequired().HasMaxLength(220);
            e.Property(x => x.ShortDescription).HasMaxLength(500);
        });

        // ---------- ProjectTechnology (many-to-many join) ----------
        modelBuilder.Entity<ProjectTechnology>(e =>
        {
            e.HasKey(x => new { x.ProjectId, x.TechnologyId });

            e.HasOne(x => x.Project)
                .WithMany(p => p.ProjectTechnologies)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Technology)
                .WithMany(t => t.ProjectTechnologies)
                .HasForeignKey(x => x.TechnologyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- Experience ----------
        modelBuilder.Entity<Experience>(e =>
        {
            e.Property(x => x.Company).IsRequired().HasMaxLength(200);
            e.Property(x => x.Position).IsRequired().HasMaxLength(200);
        });

        // ---------- Education ----------
        modelBuilder.Entity<Education>(e =>
        {
            e.Property(x => x.Institution).IsRequired().HasMaxLength(200);
            e.Property(x => x.Degree).IsRequired().HasMaxLength(200);
        });

        // ---------- Certification ----------
        modelBuilder.Entity<Certification>(e =>
        {
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Issuer).IsRequired().HasMaxLength(200);
        });

        // ---------- Achievement ----------
        modelBuilder.Entity<Achievement>(e =>
        {
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
        });

        // ---------- Service ----------
        modelBuilder.Entity<Service>(e =>
        {
            e.Property(x => x.Title).IsRequired().HasMaxLength(150);
        });

        // ---------- SocialLink ----------
        modelBuilder.Entity<SocialLink>(e =>
        {
            e.Property(x => x.Platform).IsRequired().HasMaxLength(100);
            e.Property(x => x.Url).IsRequired().HasMaxLength(500);
        });

        // ---------- ContactMessage ----------
        modelBuilder.Entity<ContactMessage>(e =>
        {
            e.Property(x => x.Name).IsRequired().HasMaxLength(150);
            e.Property(x => x.Email).IsRequired().HasMaxLength(255);
            e.Property(x => x.Message).IsRequired().HasMaxLength(4000);
            e.HasIndex(x => x.CreatedAt);
        });

        // ---------- SiteSetting ----------
        modelBuilder.Entity<SiteSetting>(e =>
        {
            e.Property(x => x.SiteTitle).IsRequired().HasMaxLength(200);
        });
        modelBuilder.Entity<Certification>(entity =>
        {
            entity.Property(x => x.IssueDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(x => x.ExpiryDate)
                .HasColumnType("timestamp without time zone");
        });
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.Property(x => x.Date)
                .HasColumnType("timestamp without time zone");
        });
        modelBuilder.Entity<Experience>(entity =>
        {
            entity.Property(x => x.StartDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(x => x.EndDate)
                .HasColumnType("timestamp without time zone");
        });
        // ---------- Education ----------
modelBuilder.Entity<Education>(entity =>
{

    entity.Property(x => x.StartDate)
        .HasColumnType("timestamp without time zone");

    entity.Property(x => x.EndDate)
        .HasColumnType("timestamp without time zone");
});
    }
}
