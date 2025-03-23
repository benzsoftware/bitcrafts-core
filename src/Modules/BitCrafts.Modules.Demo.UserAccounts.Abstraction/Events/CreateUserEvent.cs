using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;

namespace BitCrafts.Modules.Demo.UserAccounts.Abstraction.Events;

public class CreateUserEvent : BaseEvent
{
    public CreateUserEvent(User user)
    {
        User = user;
    }

    public User User { get; set; }
}