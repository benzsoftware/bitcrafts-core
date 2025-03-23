using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Data;

namespace BitCrafts.Module.Finance.Abstraction.UseCases;

public interface IDeleteBankAccountUseCase : IUseCase<IEnumerable<BankAccount>>
{
}