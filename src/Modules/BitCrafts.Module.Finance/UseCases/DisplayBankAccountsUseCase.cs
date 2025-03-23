using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.UseCases;

public sealed class DisplayBankAccountsUseCase : BaseUseCaseWithResult<IEnumerable<BankAccount>>,
    IDisplayBankAccountsUseCase
{
    private readonly FinanceDbContext _financeDbContext;

    public DisplayBankAccountsUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _financeDbContext = serviceProvider.GetRequiredService<FinanceDbContext>();
    }

    protected override async Task<IEnumerable<BankAccount>> ExecuteCoreAsync()
    {
        return await _financeDbContext.BankAccounts.ToListAsync().ConfigureAwait(false);
    }
}