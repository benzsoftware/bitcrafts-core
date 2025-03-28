﻿using Avalonia.Controls;
using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Module.Demo.UserAccounts.Extensions;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
using Material.Icons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.UserAccounts;

public sealed class UserAccountsModule : IUserAccountsModule
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
        var menuManager = serviceProvider.GetRequiredService<IMenuManager>();
        var uiManager = serviceProvider.GetRequiredService<IUiManager>();
        menuManager.AddMenuItem("Views", MaterialIconKind.ViewArray);
        menuManager.AddMenuItemInSubItem("Views", "Accounts", MaterialIconKind.About,
            () => { uiManager.ShowInTabControlAsync<IDisplayUserAccountsPresenter>(); });
        menuManager.AddSeparatorInSubItem("Views");
        menuManager.AddMenuItemInSubItem("Views", "Create account", MaterialIconKind.About,
            () => { uiManager.ShowWindowAsync<ICreateUserDialogPresenter>(); });
        var dbContext = serviceProvider.GetRequiredService<UsersDbContext>();
        dbContext.Database.Migrate();
    }
}