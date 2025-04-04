using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Module.Finance.Abstraction.Data;

namespace BitCrafts.Module.Finance.Abstraction.Views;

public interface IDisplayBankAccountsView : IView
{
    event EventHandler<BankAccount> DeleteBankAccount;
    void RefreshBankAccounts(IEnumerable<BankAccount> bankAccounts);
}