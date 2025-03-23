using BitCrafts.Infrastructure.Abstraction.Entities;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;

namespace BitCrafts.Module.Finance.Abstraction.Data;

public class BankAccount : BaseEntity
{
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<BankTransaction> Transactions { get; set; }
}