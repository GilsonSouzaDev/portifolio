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
}