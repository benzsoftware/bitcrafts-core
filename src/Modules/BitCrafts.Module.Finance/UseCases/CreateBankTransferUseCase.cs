using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.UseCases;

public sealed class CreateBankTransferUseCase : BaseUseCase<BankTransfer>, ICreateBankTransferUseCase
{
    private readonly FinanceDbContext _financeDbContext;

    public CreateBankTransferUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _financeDbContext = serviceProvider.GetRequiredService<FinanceDbContext>();
    }

    protected override async Task ExecuteCoreAsync(BankTransfer input)
    {
        // Logique spécifique pour créer un transfert
        // 1. Créer les transactions de débit et de crédit
        // 2. Créer l'entité Transfer
        await _financeDbContext.BankTransfers.AddAsync(input).ConfigureAwait(false);
        await _financeDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}