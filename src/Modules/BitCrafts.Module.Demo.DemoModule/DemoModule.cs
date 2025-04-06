using BitCrafts.Application.Abstraction.Managers;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Module.Demo.DemoModule.Data;
using BitCrafts.Module.Demo.DemoModule.Extensions;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.DemoModule;

public sealed class DemoModule : IUserAccountsModule
{
    public string Name { get; } = "UserAccounts";

    public void RegisterServices(IServiceCollection services)
    {
        services.AddUserAccountsModule();
        services.AddSingleton<IModule>(this);
    }

    public Type GetPresenterType()
    {
        return typeof(IDisplayUserAccountsPresenter);
    }

    public void Initialize(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<DemoDbContext>();
        dbContext.Database.Migrate();
    }

    public void InitializeMenus(IServiceProvider serviceProvider)
    {
        var menuManager = serviceProvider.GetRequiredService<IMenuManager>();
        var uiManager = serviceProvider.GetRequiredService<IUiManager>();
        menuManager.AddMenuItem("Views");
        menuManager.AddMenuItemInSubItem("Views", "Accounts",
            () => { uiManager.ShowInTabControlAsync<IDisplayUserAccountsPresenter>(); });
        menuManager.AddSeparatorInSubItem("Views");
        menuManager.AddMenuItemInSubItem("Views", "Create account",
            () => { uiManager.ShowWindowAsync<ICreateUserDialogPresenter>(); });
    }
}