using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;

namespace BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;

public interface ICreateUserDialogView : IView
{
    event EventHandler<User> UserCreated;
}