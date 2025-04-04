using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BitCrafts.Module.Finance.Data;

public class FinanceDbContextDesignTimeContextFactory : IDesignTimeDbContextFactory<FinanceDbContext>
{
    public FinanceDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<FinanceDbContext>();

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

        return new FinanceDbContext(builder.Options);
    }
}