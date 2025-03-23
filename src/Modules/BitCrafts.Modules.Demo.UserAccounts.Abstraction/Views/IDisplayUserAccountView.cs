using BitCrafts.Infrastructure.Abstraction.Application.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;

namespace BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;

public interface IDisplayUserAccountsView : IView
{
    void RefreshUsers(IEnumerable<User> users);
    void AppendUser(User user);
    event EventHandler CreateUser;
    event EventHandler Refresh;
    event EventHandler<User> UpdateUser;
    event EventHandler<IEnumerable<User>> DeleteUser;
}