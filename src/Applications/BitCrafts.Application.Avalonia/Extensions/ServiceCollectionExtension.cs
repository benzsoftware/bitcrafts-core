using BitCrafts.Application.Abstraction.Managers;
using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Application.Avalonia.Managers;
using BitCrafts.Application.Avalonia.Presenters;
using BitCrafts.Application.Avalonia.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BitCrafts.Application.Avalonia.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBitCraftsAvalonia(this IServiceCollection services)
    {
        services.AddManagers()
            .AddPresenters()
            .AddViews();

        return services;
    }

    private static IServiceCollection AddPresenters(this IServiceCollection services)
    {
        services.TryAddTransient<IMainPresenter, MainPresenter>();
        services.TryAddTransient<IAuthenticationPresenter, AuthenticationPresenter>();
        return services;
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.TryAddTransient<IMainView, MainView>();
        services.TryAddTransient<IAuthenticationView, AuthenticationView>();
        return services;
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.TryAddSingleton<IUiManager, AvaloniaUiManager>();
        services.TryAddSingleton<IMenuManager, AvaloniaMenuManager>();

        return services;
    }
}