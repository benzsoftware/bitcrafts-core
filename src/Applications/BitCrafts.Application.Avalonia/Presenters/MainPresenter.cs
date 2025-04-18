using BitCrafts.Application.Abstraction.Managers;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Application.Avalonia.Managers;
using BitCrafts.Application.Avalonia.Views;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Application.Avalonia.Presenters;

public sealed class MainPresenter : BasePresenter, IMainPresenter
{
    private readonly IBackgroundThreadDispatcher _backgroundThreadDispatcher;
    private readonly AvaloniaUiManager _uiManager;
    private IReadOnlyList<IModule> _modules;
    private IMainView MainView => View as MainView;

    public MainPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider, typeof(IMainView))
    {
        _uiManager = (AvaloniaUiManager)serviceProvider.GetRequiredService<IUiManager>();
        _backgroundThreadDispatcher = serviceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
        var title = serviceProvider.GetService<IConfiguration>()["ApplicationSettings:Name"] ??
                    "No Name Application";
        MainView.Title = title;
    }


    private void InitModules()
    {
        foreach (var module in _modules) module.Initialize(ServiceProvider);
    }

    private void ViewOnCloseEvent(object sender, EventArgs e)
    {
        _uiManager.CloseWindow<IMainPresenter>();
    }

    protected override async Task<IModel> LoadDataCoreAsync()
    {
        var model = await base.LoadDataCoreAsync();
        var menuManager = (AvaloniaMenuManager)ServiceProvider.GetRequiredService<IMenuManager>();
        menuManager.SetMenuControl(MainView.GetMenuControl());
        _uiManager.SetTabControl(MainView.GetTabControl());
        _modules = ServiceProvider.GetServices<IModule>().ToList().AsReadOnly();
        foreach (var module in _modules) module.InitializeMenus(ServiceProvider); // should be run on ui thread

        MainView.SetBusy(true, "Initializing Modules in background.");
        await _backgroundThreadDispatcher.InvokeAsync(InitModules);
        MainView.SetBusy(false, "");
        return model;
    }

    protected override async Task SaveChangesCoreAsync(IModel model)
    {
        await base.SaveChangesCoreAsync(model);

        await _uiManager.ShutdownAsync();
    }
}