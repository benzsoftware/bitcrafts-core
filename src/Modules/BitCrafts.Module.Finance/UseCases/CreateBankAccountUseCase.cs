using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.UseCases;

public sealed class CreateBankAccountUseCase : BaseUseCase<BankAccount>, ICreateBankAccountUseCase
{
    private readonly FinanceDbContext _financeDbContext;

    public CreateBankAccountUseCase(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _financeDbContext = serviceProvider.GetRequiredService<FinanceDbContext>();
    }

    protected override async Task ExecuteCoreAsync(BankAccount input)
    {
        await _financeDbContext.BankAccounts.AddAsync(input).ConfigureAwait(false);
        await _financeDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}