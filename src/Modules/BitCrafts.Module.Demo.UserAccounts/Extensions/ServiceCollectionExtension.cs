using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Module.Demo.UserAccounts.Presenters;
using BitCrafts.Module.Demo.UserAccounts.UseCases;
using BitCrafts.Module.Demo.UserAccounts.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.UserAccounts.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUserAccountsModule(this IServiceCollection services)
    {
        services.AddTransient<IDisplayUserAccountsPresenter, DisplayUserAccountsPresenter>();
        services.AddTransient<IDisplayUserAccountsView, DisplayUserAccountsView>();

        services.AddTransient<ICreateUserDialogPresenter, CreateUserDialogPresenter>();
        services.AddTransient<ICreateUserDialogView, CreateUserDialogView>();
        services.AddTransient<IDisplayUsersUseCase, DisplayUsersUseCase>();
        services.AddTransient<ICreateUserUseCase, CreateUserUseCase>();
        services.AddTransient<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddTransient<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddTransient<IAuthenticationUseCase, AuthenticationUseCase>();

        services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var defaultDatabaseConnectionString = configuration["ApplicationSettings:DefaultConnectionString"];
            var dbProviderName = configuration["ApplicationSettings:DbProviderName"]?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(defaultDatabaseConnectionString))
                defaultDatabaseConnectionString = "InternalDb";

            switch (dbProviderName)
            {
                case "sqlite":
                    options.UseSqlite(configuration.GetConnectionString(defaultDatabaseConnectionString));
                    break;
                case "postgresql":
                    options.UseNpgsql(configuration.GetConnectionString(defaultDatabaseConnectionString));
                    break;
            }
        });

        return services;
    }
}