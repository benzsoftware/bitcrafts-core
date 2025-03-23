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

    private void CreateMenuItems()
    {
        _modules = ServiceProvider.GetServices<IModule>().ToList().AsReadOnly();
        View.SetupMenu(_modules);
    }

    protected override async Task OnAppearedAsync()
    {
        if (View is MainView mainView) _uiManager.SetTabControl(mainView.MainTabControl);

        View.CloseEvent += ViewOnCloseEvent;
        View.MenuClickEvent += ViewOnMenuClickEvent;
        CreateMenuItems();
        await InitializeModulesAsync();
    }

    private async Task InitializeModulesAsync()
    {
        View.SetBusy("Initializing Modules ...");
        await _backgroundThreadDispatcher.InvokeTaskAsync(InitModules);
        View.UnsetBusy();
    }

    private async Task InitModules()
    {
        foreach (var module in _modules) module.Initialize(ServiceProvider);

        await Task.Delay(3000);
    }

    private void ViewOnMenuClickEvent(object sender, MenuClickEventArgs e)
    {
        if (_modules.Contains(e.Module))
        {
            var presenterType = e.Module.GetPresenterType();
            _uiManager.ShowInTabControl(presenterType, null);
        }
    }

    private void ViewOnCloseEvent(object sender, EventArgs e)
    {
        _uiManager.CloseWindow<IMainPresenter>();
    }

    protected override Task OnDisAppearedAsync()
    {
        View.CloseEvent -= ViewOnCloseEvent;
        View.MenuClickEvent -= ViewOnMenuClickEvent;
        return Task.CompletedTask;
    }
}