using Microsoft.EntityFrameworkCore;

namespace DataBases.Models;

public class OrmDbContext : DbContext
{
    public OrmDbContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = true;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
}