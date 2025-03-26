using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Abstraction.Application.Presenters;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Application.Events;
using BitCrafts.Infrastructure.Application.Managers;
using BitCrafts.Infrastructure.Application.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Infrastructure.Application.Presenters;

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
        await InitializeModulesAsync();
    }

    private async Task InitializeModulesAsync()
    {
        View.SetBusy("Initializing Modules ...");
        await _backgroundThreadDispatcher.InvokeTaskAsync(InitModules);
        View.UnsetBusy();
    }

    private Task InitModules()
    {
        foreach (var module in _modules) module.Initialize(ServiceProvider);
        return Task.CompletedTask;
    }
 
    private void ViewOnCloseEvent(object sender, EventArgs e)
    {
        _uiManager.CloseWindow<IMainPresenter>();
    }

    protected override async Task OnDisAppearedAsync()
    {
        View.CloseEvent -= ViewOnCloseEvent;
        await ServiceProvider.GetRequiredService<IUiManager>().ShutdownAsync();
    }
}