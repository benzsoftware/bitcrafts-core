using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BitCrafts.Module.Demo.DemoModule.Data;

public class DemoDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
{
    public DemoDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DemoDbContext>();
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

        return new DemoDbContext(builder.Options);
    }
}