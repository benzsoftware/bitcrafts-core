using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Module.Demo.UserAccounts.Extensions;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
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
        var dbContext = serviceProvider.GetRequiredService<UsersDbContext>();
        dbContext.Database.Migrate();
    }
}