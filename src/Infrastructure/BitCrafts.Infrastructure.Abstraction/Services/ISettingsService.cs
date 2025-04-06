namespace BitCrafts.Infrastructure.Abstraction.Services;

public interface ISettingsService
{
    Task LoadAsync();
    Task SaveAsync();

    T GetValue<T>(string key);
    void SetValue<T>(string key, T value);

    bool HasKey(string key);
    void RemoveKey(string key);
}