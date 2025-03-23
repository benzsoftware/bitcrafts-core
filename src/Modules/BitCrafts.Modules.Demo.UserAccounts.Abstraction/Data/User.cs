using BitCrafts.Infrastructure.Abstraction.Entities;

namespace BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
}