using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Models;

namespace BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;

public interface IDisplayUserAccountsView : ILoadableView<DisplayAccountsModel>
{
    /* void RefreshUsers(IEnumerable<User> users);
     void AppendUser(User user);*/
    /*event EventHandler CreateUser;
    event EventHandler Refresh;
    event EventHandler<User> UpdateUser;
    event EventHandler<IEnumerable<User>> DeleteUser;*/
}