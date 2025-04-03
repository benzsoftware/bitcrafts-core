using System.Globalization;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using BitCrafts.Application.Abstraction.Application.Managers;
using BitCrafts.Application.Avalonia.Extensions;
using BitCrafts.Application.Avalonia.Managers;
using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Infrastructure.Extensions;
using BitCrafts.Infrastructure.Threading;
using Material.Icons.Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Semi.Avalonia;

namespace BitCrafts.Application.Avalonia;

public abstract class BaseApplication : global::Avalonia.Application
{
    private BackgroundThreadDispatcher _backgroundThreadDispatcher;
    private IServiceProvider ServiceProvider { get; set; }

    public override void Initialize()
    {
        RequestedThemeVariant = ThemeVariant.Light;
        Styles.Add(new SemiTheme() { Locale = new CultureInfo("en-US") });
        Styles.Add(new SemiPopupAnimations());

        Styles.Add(new StyleInclude(new Uri("avares://BitCrafts.Infrastructure/"))
        {
            Source = new Uri("avares://Semi.Avalonia.ColorPicker/Index.axaml")
        });
        Styles.Add(new StyleInclude(new Uri("avares://BitCrafts.Infrastructure/"))
        {
            Source = new Uri("avares://Semi.Avalonia.DataGrid/Index.axaml")
        });
        Styles.Add(new StyleInclude(new Uri("avares://BitCrafts.Infrastructure/"))
        {
            Source = new Uri("avares://Semi.Avalonia.TreeDataGrid/Index.axaml")
        });
        Styles.Add(new MaterialIconStyles(null));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddBitCraftsInfrastructure();
        services.AddBitCraftsAvalonia();

        ServiceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
            var uiManager = (AvaloniaUiManager)ServiceProvider.GetRequiredService<IUiManager>();
            _backgroundThreadDispatcher =
                (BackgroundThreadDispatcher)ServiceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
            _backgroundThreadDispatcher.Start();

            try
            {
                //verify if there is an implementation of IauthenticationUseCase
                ServiceProvider.GetRequiredService<IAuthenticationUseCase>();
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