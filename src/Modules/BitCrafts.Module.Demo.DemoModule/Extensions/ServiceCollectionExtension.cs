using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.DemoModule.Data;
using BitCrafts.Module.Demo.DemoModule.UseCases; 
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.DemoModule.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUserAccountsModule(this IServiceCollection services)
    {
        services.AddTransient<IDisplayUsersUseCase, DisplayUsersUseCase>();
        services.AddTransient<ICreateUserUseCase, CreateUserUseCase>();
        services.AddTransient<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddTransient<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddTransient<IAuthenticationUseCase, AuthenticationUseCase>();

        services.AddDbContext<DemoDbContext>((serviceProvider, options) =>
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