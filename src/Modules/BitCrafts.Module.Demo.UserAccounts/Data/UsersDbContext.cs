using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using Microsoft.EntityFrameworkCore;

namespace BitCrafts.Module.Demo.UserAccounts.Data;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}