using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.UseCases;

public sealed class DisplayBankTransactionsUseCase : BaseUseCaseWithResult<IEnumerable<BankTransaction>>,
    IDisplayBankTransactionsUseCase

{
    private readonly FinanceDbContext _financeDbContext;

    public DisplayBankTransactionsUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _financeDbContext = serviceProvider.GetRequiredService<FinanceDbContext>();
    }

    protected override async Task<IEnumerable<BankTransaction>> ExecuteCoreAsync()
    {
        return await _financeDbContext.BankTransactions.ToListAsync().ConfigureAwait(false);
    }
}