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
        var connectionString = configuration.GetConnectionString("InternalDb");
        builder.UseSqlite(connectionString);

        return new FinanceDbContext(builder.Options);
    }
}