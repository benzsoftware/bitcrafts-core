using ActiproSoftware.UI.Avalonia.Themes;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Avalonia.Themes.Simple;
using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Infrastructure.Application.Managers;
using BitCrafts.Infrastructure.Extensions;
using BitCrafts.Infrastructure.Threading;
using Material.Icons.Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Infrastructure;

public abstract class BaseApplication : global::Avalonia.Application
{
    private BackgroundThreadDispatcher _backgroundThreadDispatcher;
    public static IServiceProvider ServiceProvider { get; private set; }

    public override void Initialize()
    {
        RequestedThemeVariant = ThemeVariant.Dark;
        Styles.Add(new FluentTheme());
        Styles.Add(new ModernTheme());
        Styles.Add(new StyleInclude(new Uri("avares://BitCrafts.Infrastructure/"))
        {
            Source = new Uri("avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml")
        }); 
        Styles.Add(new MaterialIconStyles(null));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddBitCraftsInfrastructure();

        ServiceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var uiManager = (AvaloniaUiManager)ServiceProvider.GetRequiredService<IUiManager>();
            _backgroundThreadDispatcher =
                (BackgroundThreadDispatcher)ServiceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
            _backgroundThreadDispatcher.Start();
            desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;

            //verify if there is an implementation of IauthenticationUseCase
            try
            {
                var authUseCase = ServiceProvider.GetRequiredService<IAuthenticationUseCase>();
                uiManager.SetNativeApplication(desktop, true);
            }
            catch (Exception)
            {
                uiManager.SetNativeApplication(desktop, false);
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}