using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.UseCases;

public sealed class DisplayBankTransfersUseCase : BaseUseCaseWithResult<IEnumerable<BankTransfer>>,
    IDisplayBankTransfersUseCase

{
    private readonly FinanceDbContext _financeDbContext;

    public DisplayBankTransfersUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _financeDbContext = serviceProvider.GetRequiredService<FinanceDbContext>();
    }

    protected override async Task<IEnumerable<BankTransfer>> ExecuteCoreAsync()
    {
        return await _financeDbContext.BankTransfers.ToListAsync().ConfigureAwait(false);
    }
}