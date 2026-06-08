using Microsoft.EntityFrameworkCore;

namespace Portfolio.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}