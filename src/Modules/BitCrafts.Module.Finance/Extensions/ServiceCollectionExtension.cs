using BitCrafts.Module.Finance.Abstraction.Presenters;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Views;
using BitCrafts.Module.Finance.Data;
using BitCrafts.Module.Finance.Presenters;
using BitCrafts.Module.Finance.UseCases;
using BitCrafts.Module.Finance.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BitCrafts.Module.Finance.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFinanceModule(this IServiceCollection services)
    {
        services.TryAddTransient<IFinanceDashboardView, FinanceDashboardView>();
        services.TryAddTransient<IFinanceDahsboardPresenter, FinanceDashboardPresenter>();

        services.TryAddTransient<ICreateBankAccountPresenter, CreateBankAccountPresenter>();
        services.TryAddTransient<ICreateBankAccountView, CreateBankAccountView>();

        services.TryAddTransient<IDisplayBankAccountsPresenter, DisplayBankAccountsPresenter>();
        services.TryAddTransient<IDisplayBankAccountsView, DisplayBankAccountsView>();

        services.TryAddTransient<ICreateBankTransactionUseCase, CreateBankTransactionUseCase>();
        services.TryAddTransient<ICreateBankAccountUseCase, CreateBankAccountUseCase>();
        services.TryAddTransient<ICreateBankTransferUseCase, CreateBankTransferUseCase>();
        services.TryAddTransient<IDisplayBankTransactionsUseCase, DisplayBankTransactionsUseCase>();
        services.TryAddTransient<IDisplayBankAccountsUseCase, DisplayBankAccountsUseCase>();
        services.TryAddTransient<IDisplayBankTransfersUseCase, DisplayBankTransfersUseCase>();
        services.TryAddTransient<IDeleteBankAccountUseCase, DeleteBankAccountUseCase>();
        services.TryAddTransient<IUpdateBankAccountUseCase, UpdateBankAccountUseCase>();

        services.AddDbContext<FinanceDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var defaultDatabaseConnectionString = configuration["ApplicationSettings:DefaultConnectionString"];
            if (string.IsNullOrWhiteSpace(defaultDatabaseConnectionString))
                defaultDatabaseConnectionString = "InternalDb";

            var dbProviderName = configuration["ApplicationSettings:DbProviderName"]?.ToLowerInvariant();
            switch (dbProviderName)
            {
                case "sqlite":
                    options.UseSqlite(configuration.GetConnectionString(defaultDatabaseConnectionString));
                    break;
            }
        });

        return services;
    }
}