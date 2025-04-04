using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BitCrafts.Module.Demo.UserAccounts.Data;

public class UsersDesignTimeDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<UsersDbContext>();
        var dbProviderName = configuration["ApplicationSettings:DbProviderName"]?.ToLowerInvariant();
        switch (dbProviderName)
        {
            case "sqlite":
                builder.UseSqlite(configuration.GetConnectionString(dbProviderName));
                break;
            case "postgresql":
                builder.UseNpgsql(configuration.GetConnectionString(dbProviderName));
                break;
        }

        return new UsersDbContext(builder.Options);
    }
}