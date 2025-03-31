using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Module.Finance.Abstraction;
using BitCrafts.Module.Finance.Abstraction.Presenters;
using BitCrafts.Module.Finance.Data;
using BitCrafts.Module.Finance.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance;

public sealed class FinanceModule : IFinanceModule
{
    public string Name => "Finance";

    public void RegisterServices(IServiceCollection services)
    {
        services.AddFinanceModule();
        services.AddSingleton<IModule>(this);
    }

    public Type GetPresenterType()
    {
        return typeof(IFinanceDahsboardPresenter);
    }

    public void Initialize(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<FinanceDbContext>();
        dbContext.Database.Migrate();
    }

    public void InitializeMenus(IServiceProvider serviceProvider)
    {
    }
}