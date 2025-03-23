using ActiproSoftware.UI.Avalonia.Themes;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Avalonia.Themes.Simple;
using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Application.Managers;
using BitCrafts.Infrastructure.Extensions;
using Material.Icons.Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Infrastructure;

public abstract class BaseApplication : global::Avalonia.Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public override void Initialize()
    {
        RequestedThemeVariant = ThemeVariant.Dark;
        Styles.Add(new SimpleTheme());
        Styles.Add(new FluentTheme());
        Styles.Add(new ModernTheme());
        Styles.Add(new StyleInclude(new Uri("avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml"))
        {
            Source = new Uri("avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml")
        });
        Styles.Add(new MaterialIconStyles(null));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services
            .AddBitCraftsInfrastructure();

        ServiceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var uiManager = (AvaloniaUiManager)ServiceProvider.GetRequiredService<IUiManager>();
            desktop.ShutdownMode = ShutdownMode.OnLastWindowClose;
            uiManager.SetNativeApplication(desktop);
        }

        base.OnFrameworkInitializationCompleted();
    }
}