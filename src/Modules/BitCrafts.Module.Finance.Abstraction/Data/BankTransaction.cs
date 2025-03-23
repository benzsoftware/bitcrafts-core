using BitCrafts.Infrastructure.Abstraction.Entities;

namespace BitCrafts.Module.Finance.Abstraction.Data;

public class BankTransaction : BaseEntity
{
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public int BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; }
}