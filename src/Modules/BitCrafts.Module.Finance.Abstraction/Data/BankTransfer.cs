using BitCrafts.Infrastructure.Abstraction.Entities;

namespace BitCrafts.Module.Finance.Abstraction.Data;

public class BankTransfer : BaseEntity
{
    public int SourceAccountId { get; set; }
    public BankAccount SourceAccount { get; set; }
    public int DestinationAccountId { get; set; }
    public BankAccount DestinationAccount { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransferDate { get; set; }
    public BankTransaction SourceTransaction { get; set; }
    public BankTransaction DestinationTransaction { get; set; }
}