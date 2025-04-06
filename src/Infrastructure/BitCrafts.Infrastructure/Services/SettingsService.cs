using System.Text.Json;
using BitCrafts.Infrastructure.Abstraction.Services;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Services;

public sealed class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;
    private readonly string _settingsFolder = "Settings  ";
    private readonly string _settingsFile = "applicationSettings.json";
    private readonly JsonSerializerOptions _jsonOptions;
    private Dictionary<string, object> _settingsCache;
    private bool _isDirty = false;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;

        _settingsCache = new Dictionary<string, object>();


        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    private async Task InitializeCacheAsync(string filePath)
    {
        if (File.Exists(filePath))
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, _jsonOptions);

                if (result != null)
                {
                    _settingsCache = new Dictionary<string, object>();
                    foreach (var item in result) _settingsCache[item.Key] = item.Value;
                }

                _logger.LogInformation($"Loaded {_settingsCache.Count} application settings");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load application settings");
            }
    }

    public async Task LoadAsync()
    {
        var basePath = Environment.GetEnvironmentVariable("SNAP_USER_DATA")
                       ?? AppContext.BaseDirectory;
        var directoryPath = Path.Combine(basePath, _settingsFolder);
        var filePath = Path.Combine(directoryPath, _settingsFile);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);


        await InitializeCacheAsync(filePath).ConfigureAwait(false);
    }

    public async Task SaveAsync()
    {
        if (!_isDirty)
            return;

        try
        {
            var json = JsonSerializer.Serialize(_settingsCache, _jsonOptions);
            await File.WriteAllTextAsync(_settingsFile, json).ConfigureAwait(false);
            _isDirty = false;
            _logger.LogInformation("Saved application settings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save application settings");
            throw;
        }
    }

    public T GetValue<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        if (_settingsCache.TryGetValue(key, out var value))
        {
            if (value is JsonElement element)
                try
                {
                    return JsonSerializer.Deserialize<T>(element.GetRawText(), _jsonOptions);
                }
                catch
                {
                    return default;
                }

            if (value is T typedValue) return typedValue;

            try
            {
                var json = JsonSerializer.Serialize(value, _jsonOptions);
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
            catch
            {
                return default;
            }
        }

        return default;
    }

    public void SetValue<T>(string key, T value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        _settingsCache[key] = value;
        _isDirty = true;
    }

    public bool HasKey(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        return _settingsCache.ContainsKey(key);
    }

    public void RemoveKey(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        if (_settingsCache.ContainsKey(key))
        {
            _settingsCache.Remove(key);
            _isDirty = true;
        }
    }
}