using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Abstraction.Application.Presenters;
using BitCrafts.Infrastructure.Abstraction.Application.Views;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Infrastructure.Abstraction.Services;
using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Application.Managers;
using BitCrafts.Infrastructure.Application.Presenters;
using BitCrafts.Infrastructure.Application.Views;
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

        services.TryAddSingleton<IBackgroundThreadDispatcher, BackgroundThreadDispatcher>();
        services.TryAddSingleton<IParallelism, Parallelism>();
        services.TryAddSingleton<IEventAggregator, EventAggregator>();
        services.TryAddSingleton<IHashingService, HashingService>();
        services.TryAddSingleton<IUiManager, AvaloniaUiManager>();
        services.TryAddSingleton<IMenuManager, AvaloniaMenuManager>();
        services.TryAddSingleton<IDataValidator, DataValidator>();
        services.TryAddTransient<IMainPresenter, MainPresenter>();
        services.TryAddTransient<IMainView, MainView>();
        services.TryAddTransient<IAuthenticationView, AuthenticationView>();
        services.TryAddTransient<IAuthenticationPresenter, AuthenticationPresenter>();
        CreateDirectory("Modules");
        CreateDirectory("Settings");
        CreateDirectory("Databases");
        CreateDirectory("Files");
        CreateDirectory("Temporary");
        var moduleRegistrer = new ModuleRegistrer(Log.Logger);
        moduleRegistrer.RegisterModules(services);
        return services;
    }

    private static void CreateDirectory(string directory)
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), directory);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(Path.GetFullPath(directoryPath));
    }
}