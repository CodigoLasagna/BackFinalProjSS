using Domain;
using Microsoft.EntityFrameworkCore;

namespace Helper.Connection;

public class DBContextController : DbContext
{

    public DBContextController() { }
    public DBContextController(DbContextOptions<DBContextController> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Constants.ConnectionString;
        optionsBuilder.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString));
    }
    public DbSet<User> Users { get; set; }
    public DbSet<PhishUser> PhishUsers { get; set; }
    public DbSet<RsaKey> RsaKeys { get; set; }
}
public class DbConnection
{
    public static DbContextOptions<DBContextController> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<DBContextController>()
            .UseMySql(Constants.ConnectionString,
                ServerVersion.AutoDetect(Constants.ConnectionString))
            .Options;
    }
}
