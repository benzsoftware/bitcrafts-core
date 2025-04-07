namespace BitCrafts.Infrastructure.Abstraction.Services;

public class EnvironmentConfiguration
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "Production Environment";
    public string Description { get; set; } = "Default Environment";
    public EnvironmentType Type { get; set; } = EnvironmentType.Production;
    public DatabaseProviderType DatabaseProvider { get; set; } = DatabaseProviderType.SqlLite;
    public string ConnectionString { get; set; } = "Data Source=Databases/internal.db";
    public bool IsDefault { get; set; } = true;
}

public enum EnvironmentType
{
    Development,
    Testing,
    Staging,
    Production
}

public enum DatabaseProviderType
{
    SqlLite,
    PostgreSql
}

public interface IEnvironmentConfigurationService
{
    Task<List<EnvironmentConfiguration>> GetEnvironmentsAsync();
    Task<EnvironmentConfiguration> GetEnvironmentByIdAsync(string id);
    Task<EnvironmentConfiguration> GetDefaultEnvironmentAsync();
    Task SaveEnvironmentAsync(EnvironmentConfiguration environment);
    Task DeleteEnvironmentAsync(string id);
    Task SetDefaultEnvironmentAsync(string id);
    Task<bool> HasAnyEnvironmentAsync();
}