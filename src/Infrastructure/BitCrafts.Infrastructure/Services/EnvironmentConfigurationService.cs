using System.Text.Json;
using BitCrafts.Infrastructure.Abstraction.Services;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Services;

public class EnvironmentConfigurationService : IEnvironmentConfigurationService
{
    private readonly string _configFilePath;
    private readonly ILogger<EnvironmentConfigurationService> _logger;

    public EnvironmentConfigurationService(ILogger<EnvironmentConfigurationService> logger)
    {
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
            return new List<EnvironmentConfiguration>();

        try
        {
            var json = await File.ReadAllTextAsync(_configFilePath);
            var environments = JsonSerializer.Deserialize<List<EnvironmentConfiguration>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return environments ?? new List<EnvironmentConfiguration>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading environments configuration");
            return new List<EnvironmentConfiguration>();
        }
    }

    public async Task<EnvironmentConfiguration> GetEnvironmentByIdAsync(string id)
    {
        var environments = await GetEnvironmentsAsync();
        return environments.FirstOrDefault(e => e.Id == id);
    }

    public async Task<EnvironmentConfiguration> GetDefaultEnvironmentAsync()
    {
        var environments = await GetEnvironmentsAsync();
        return environments.FirstOrDefault(e => e.IsDefault);
    }

    public async Task SaveEnvironmentAsync(EnvironmentConfiguration environment)
    {
        var environments = await GetEnvironmentsAsync();
        var existingEnv = environments.FirstOrDefault(e => e.Id == environment.Id);

        if (existingEnv != null)
        {
            // Update existing
            var index = environments.IndexOf(existingEnv);
            environments[index] = environment;
        }
        else
        {
            // Add new
            environments.Add(environment);
        }

        // If this is the first environment or marked as default
        if (environments.Count == 1 || environment.IsDefault)
            foreach (var env in environments)
                env.IsDefault = env.Id == environment.Id;

        await SaveEnvironmentsAsync(environments);
    }

    public async Task DeleteEnvironmentAsync(string id)
    {
        var environments = await GetEnvironmentsAsync();
        var envToRemove = environments.FirstOrDefault(e => e.Id == id);

        if (envToRemove != null)
        {
            environments.Remove(envToRemove);

            // If we removed the default environment, set a new default
            if (envToRemove.IsDefault && environments.Count > 0) environments[0].IsDefault = true;

            await SaveEnvironmentsAsync(environments);
        }
    }

    public async Task SetDefaultEnvironmentAsync(string id)
    {
        var environments = await GetEnvironmentsAsync();

        foreach (var env in environments) env.IsDefault = env.Id == id;

        await SaveEnvironmentsAsync(environments);
    }

    public async Task<bool> HasAnyEnvironmentAsync()
    {
        var environments = await GetEnvironmentsAsync();
        return environments.Count > 0;
    }

    private async Task SaveEnvironmentsAsync(List<EnvironmentConfiguration> environments)
    {
        try
        {
            var json = JsonSerializer.Serialize(environments,
                new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_configFilePath, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving environments configuration");
            throw;
        }
    }
}