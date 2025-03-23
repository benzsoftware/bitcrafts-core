using BitCrafts.Module.Finance.Abstraction.Data;
using Microsoft.EntityFrameworkCore;

namespace BitCrafts.Module.Finance.Data;

public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options)
    {
    }

    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<BankTransaction> BankTransactions { get; set; }
    public DbSet<BankTransfer> BankTransfers { get; set; }
}