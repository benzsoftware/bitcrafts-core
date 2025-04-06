using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;

namespace BitCrafts.Modules.Demo.UserAccounts.Abstraction.Models;

public class DisplayAccountsModel : BaseViewModel
{
    public List<User> Users { get; set; } = new();
    public User SelectedUser { get; set; }
}