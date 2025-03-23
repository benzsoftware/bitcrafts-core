using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.UseCases;

public sealed class CreateBankTransactionUseCase : BaseUseCase<BankTransaction>, ICreateBankTransactionUseCase
{
    private readonly FinanceDbContext _financeDbContext;

    public CreateBankTransactionUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _financeDbContext = serviceProvider.GetRequiredService<FinanceDbContext>();
    }

    protected override async Task ExecuteCoreAsync(BankTransaction input)
    {
        await _financeDbContext.BankTransactions.AddAsync(input).ConfigureAwait(false);
        await _financeDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}