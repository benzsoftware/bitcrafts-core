using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using Microsoft.EntityFrameworkCore;

namespace BitCrafts.Module.Demo.DemoModule.Data;

public class DemoDbContext : DbContext
{
    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}