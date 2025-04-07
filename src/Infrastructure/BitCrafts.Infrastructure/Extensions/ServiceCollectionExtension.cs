using System.Diagnostics.CodeAnalysis;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Infrastructure.Abstraction.Services;
using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Data;
using BitCrafts.Infrastructure.Events;
using BitCrafts.Infrastructure.Modules;
using BitCrafts.Infrastructure.Services;
using BitCrafts.Infrastructure.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace BitCrafts.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBitCraftsInfrastructure(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        Log.Logger = logger;

        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true)
        );

        services.AddManagers();

        CreateDirectory("Modules");
        CreateDirectory("Settings");
        CreateDirectory("Databases");
        CreateDirectory("Files");
        CreateDirectory("Temporary");
        var moduleRegistrer = new ModuleRegistrer(Log.Logger);
        moduleRegistrer.RegisterModules(services);
        return services;
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.TryAddSingleton<IBackgroundThreadDispatcher, BackgroundThreadDispatcher>();
        services.TryAddSingleton<IHashingService, HashingService>();
        services.TryAddSingleton<IDataValidator, DataValidator>();
        services.TryAddSingleton<IEnvironmentConfigurationService, EnvironmentConfigurationService>();
        services.TryAddTransient<IEventAggregator, EventAggregator>();
        return services;
    }

    private static void CreateDirectory(string directory)
    {
        var basePath = Environment.GetEnvironmentVariable("SNAP_USER_DATA")
                       ?? AppContext.BaseDirectory;

        var directoryPath = Path.Combine(basePath, directory);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(Path.GetFullPath(directoryPath));
    }
}