using Microsoft.EntityFrameworkCore;
using TransactionService.DbContexts;
using TransactionService.Models;
using TransactionService.Repositories.Contracts;

namespace TransactionService.Repositories
{
    //Repository for Transactions table
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly InMemoryDbContext _dbContext;
        public TransactionsRepository(InMemoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieve transactions list from DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<Transaction>> GetTransactions()
        {
            return await _dbContext.Transactions.ToListAsync();
        }

        /// <summary>
        /// Retrieve Transactions in an account from DB
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<Transaction>> GetTransactionsByAccountId(int accountId)
        {
            return await _dbContext.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
        }

        /// <summary>
        /// Record a transaction to DB
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<int> RecordTransaction(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction.Id;
        }
    }
}
