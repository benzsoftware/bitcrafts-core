using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using BitCrafts.Infrastructure.Abstraction.Modules;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BitCrafts.Infrastructure.Modules;

[ExcludeFromCodeCoverage]
public sealed class ModuleRegistrer : IModuleRegistrer
{
    private readonly ILogger _logger;
    private List<Assembly> _loadedAssemblies;

    public ModuleRegistrer(ILogger logger)
    {
        _logger = logger;
        _loadedAssemblies = new List<Assembly>();
    }

    public void RegisterModules(IServiceCollection services)
    {
        var currentPath = Directory.GetCurrentDirectory();
        var tempPath = Path.Combine(currentPath, "Modules");
        var modulesPath = Path.IsPathRooted(tempPath) ? tempPath : Path.GetFullPath(tempPath);

        LoadModulesFromPath(modulesPath, services);
        LoadModulesFromPath(currentPath, services);
    }

    public void Dispose()
    {
        _loadedAssemblies.Clear();
        _loadedAssemblies = null;
    }

    private void LoadModulesFromPath(string path, IServiceCollection services)
    {
        if (!string.IsNullOrEmpty(path) || !Directory.Exists(path))
        {
            var allFiles = Directory.GetFiles(path, "*.Module.*.dll");
            if (allFiles.Length <= 0)
            {
                _logger.Warning($"{path} has as no modules.");
                return;
            }

            //loading the modules contracts dll;
            foreach (var dll in allFiles.Where(x => x.Contains("Abstraction")))
            {
                var dllName = Path.GetFileName(dll);
                LoadAssembly(dll, services);
                _logger.Information($"Loaded assembly {dllName} OK.");
            }

            //loading the modules implementation dll;
            foreach (var dll in allFiles.Where(x => !x.Contains("Abstraction")))
            {
                var dllName = Path.GetFileName(dll);
                LoadAssembly(dll, services, true);
                _logger.Information($"Loaded assembly {dllName} OK.");
            }
        }
    }

    private void LoadAssembly(string dll, IServiceCollection services, bool registerAsModule = false)
    {
        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
        _loadedAssemblies.Add(assembly);
        if (registerAsModule) RegisterModules(assembly, services);
    }

    private void RegisterModules(Assembly assembly, IServiceCollection services)
    {
        try
        {
            var moduleTypes = assembly.GetTypes().Where(IsValidModule);
            foreach (var type in moduleTypes)
                if (Activator.CreateInstance(type) is IModule moduleInstance)
                    moduleInstance.RegisterServices(services);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error loading assembly {assembly.FullName}");
        }
    }

    private bool IsValidClass(Type type)
    {
        return type.IsClass && !type.IsAbstract;
    }

    private bool IsValidModule(Type type)
    {
        return IsValidClass(type) && typeof(IModule).IsAssignableFrom(type);
    }
}