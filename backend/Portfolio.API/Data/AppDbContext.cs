using Microsoft.EntityFrameworkCore;
using Portfolio.API.Models;

namespace Portfolio.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<MagicLinkToken> MagicLinkTokens { get; set; }
    public DbSet<SocialLink> SocialLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(bool) || property.ClrType == typeof(bool?))
                {
                    property.SetColumnType("NUMBER(1)");
                }
            }
        }
    }
}