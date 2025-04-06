using BitCrafts.Application.Abstraction.Managers;
using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Application.Avalonia.Managers;
using BitCrafts.Application.Avalonia.Views;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Application.Avalonia.Presenters;

public sealed class MainPresenter : BasePresenter<IMainView>, IMainPresenter
{
    private readonly IBackgroundThreadDispatcher _backgroundThreadDispatcher;
    private readonly AvaloniaUiManager _uiManager;
    private IReadOnlyList<IModule> _modules;

    public MainPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _uiManager = (AvaloniaUiManager)serviceProvider.GetRequiredService<IUiManager>();

        _backgroundThreadDispatcher = serviceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
        var title = serviceProvider.GetService<IConfiguration>()["ApplicationSettings:Name"] ??
                    "No Name Application";
        View.Title = title;
    }

    protected override async Task OnAppearedAsync()
    {
        View.CloseEvent += ViewOnCloseEvent;
        var menuManager = (AvaloniaMenuManager)ServiceProvider.GetRequiredService<IMenuManager>();
        menuManager.SetMenuControl(View.GetMenuControl());
        _uiManager.SetTabControl(View.GetTabControl());
        _modules = ServiceProvider.GetServices<IModule>().ToList().AsReadOnly();
        foreach (var module in _modules) module.InitializeMenus(ServiceProvider); // should be run on ui thread

        await InitializeModulesAsync();
    }

    private async Task InitializeModulesAsync()
    {
        View.SetBusy("Initializing Modules in background.");
        await _backgroundThreadDispatcher.InvokeAsync(InitModules);
        View.UnsetBusy();
    }

    private void InitModules()
    {
        foreach (var module in _modules) module.Initialize(ServiceProvider);
    }

    private void ViewOnCloseEvent(object sender, EventArgs e)
    {
        _uiManager.CloseWindow<IMainPresenter>();
    }

    protected override async Task OnDisappearedAsync()
    {
        View.CloseEvent -= ViewOnCloseEvent;
        await ServiceProvider.GetRequiredService<IUiManager>().ShutdownAsync();
    }
}