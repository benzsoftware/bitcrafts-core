using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.UseCases;

public sealed class DeleteBankAccountUseCase : BaseUseCase<IEnumerable<BankAccount>>, IDeleteBankAccountUseCase
{
    private readonly FinanceDbContext _financeDbContext;

    public DeleteBankAccountUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _financeDbContext = serviceProvider.GetRequiredService<FinanceDbContext>();
    }

    protected override async Task ExecuteCoreAsync(IEnumerable<BankAccount> input)
    {
        _financeDbContext.BankAccounts.RemoveRange(input);
        await _financeDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}