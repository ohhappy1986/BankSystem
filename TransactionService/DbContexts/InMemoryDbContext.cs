using Microsoft.EntityFrameworkCore;
using TransactionService.DbContexts.Contracts;
using TransactionService.Models;

namespace TransactionService.DbContexts
{
    /// <summary>
    /// DbContext for In-Memory DB
    /// </summary>
    public class InMemoryDbContext : DbContext, IInMemoryDbContext
    {
        public InMemoryDbContext() { }
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
        {
            loadInitialData();
        }
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// For demo/test purpose only, populate sample data.
        /// </summary>
        private void loadInitialData()
        {
            //Load initial Transactions data for demo purpose.
            List<Transaction> transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                AccountId = 1,
                TransactionType = true,
                TransactionAmount = 20000.00M
            },
            new Transaction
            {
                Id = 2,
                AccountId = 1,
                TransactionType = false,
                TransactionAmount = 5000.00M
            },
            new Transaction
            {
                Id = 3,
                AccountId = 2,
                TransactionType = true,
                TransactionAmount = 30000.00M
            },
            new Transaction
            {
                Id = 4,
                AccountId = 2,
                TransactionType = false,
                TransactionAmount = 7000.00M
            },
            new Transaction
            {
                Id = 5,
                AccountId = 3,
                TransactionType = true,
                TransactionAmount = 40000.00M
            },
            new Transaction
            {
                Id = 6,
                AccountId = 3,
                TransactionType = false,
                TransactionAmount = 4000.00M
            },
        };
            this.Transactions.AddRange(transactions);

            this.SaveChanges();
        }
    }
}
