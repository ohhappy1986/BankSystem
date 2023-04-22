using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

namespace TransactionService.DbContexts.Contracts
{
    public interface IInMemoryDbContext
    {
        DbSet<Transaction> Transactions { get; set; }
    }
}
