using Microsoft.EntityFrameworkCore;

namespace Core.Database;

public class CoreContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<StoredUserTokens> StoredUserTokens { get; set; } = null!;
    public DbSet<Teacher> Teachers { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;

    public CoreContext(DbContextOptions options) : base(options)
    {
    }
}
