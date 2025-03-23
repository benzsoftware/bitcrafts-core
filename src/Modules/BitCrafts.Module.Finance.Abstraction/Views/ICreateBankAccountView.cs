using BitCrafts.Infrastructure.Abstraction.Application.Views;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;

namespace BitCrafts.Module.Finance.Abstraction.Views;

public interface ICreateBankAccountView : IView
{
    event EventHandler<BankAccount> CreateBankAccount;
    event EventHandler CloseView;

    void ClearFields();

    void RefreshUsers(IEnumerable<User> users);
}