using System.Text.Json;
using BitCrafts.Infrastructure.Abstraction.Services;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Services;

public class EnvironmentConfigurationService : IEnvironmentConfigurationService
{
    private readonly string _configFilePath;
    private readonly ILogger<EnvironmentConfigurationService> _logger;
    private List<EnvironmentConfiguration> _environmentConfigurations;

    public EnvironmentConfigurationService(ILogger<EnvironmentConfigurationService> logger)
    {
        _environmentConfigurations = new List<EnvironmentConfiguration>();
        var basePath = Environment.GetEnvironmentVariable("SNAP_USER_DATA")
                       ?? AppContext.BaseDirectory;

        var settingsBasePath = Path.Combine(basePath, "Settings");
        if (!Directory.Exists(settingsBasePath))
            Directory.CreateDirectory(settingsBasePath);

        _configFilePath = Path.Combine(settingsBasePath, "environments.json");
        _logger = logger;
    }

    public async Task<List<EnvironmentConfiguration>> GetEnvironmentsAsync()
    {
        if (!File.Exists(_configFilePath))
            return _environmentConfigurations;

        try
        {
            var json = await File.ReadAllTextAsync(_configFilePath);
            var environments = JsonSerializer.Deserialize<List<EnvironmentConfiguration>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _environmentConfigurations = environments ?? new List<EnvironmentConfiguration>();
            return _environmentConfigurations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading environments configuration");
            return new List<EnvironmentConfiguration>();
        }
    }
 
    public async Task<bool> HasAnyEnvironmentAsync()
    {
        var environments = await GetEnvironmentsAsync();
        return environments.Count > 0;
    }

    public async Task<bool> SaveEnvironmentsAsync(List<EnvironmentConfiguration> environments)
    {
        try
        {
            var json = JsonSerializer.Serialize(environments,
                new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_configFilePath, json).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving environments configuration");
            return false;
        }
    }
}