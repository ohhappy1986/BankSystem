using AccountService.DbContexts;
using AccountService.Models;
using AccountService.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repositories
{
    /// <summary>
    /// Repository for Accounts table
    /// </summary>
    public class AccountsRepository : IAccountsRepository
    {
        private readonly InMemoryDbContext _dbContext;
        public AccountsRepository(InMemoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all accounts.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Account>> GetAccounts()
        {
            return await _dbContext.Accounts.ToListAsync();
        }

        /// <summary>
        /// Get accounts of an user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Account>> GetAccountsByUserId(int userId)
        {
            return await _dbContext.Accounts.Where(a => a.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Get an account by account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Account> GetAccountById(int accountId)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        }

        /// <summary>
        /// Save new account to DB
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<int> CreateAccount(Account account)
        {
            if (account != null)
            {
                await _dbContext.Accounts.AddAsync(account);
                await _dbContext.SaveChangesAsync();
                return account.Id;
            }
            return -1;
        }

        /// <summary>
        /// Remove account from DB
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Account> DeleteAccount(int accountId)
        {
            Account delAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (delAccount != null)
            {
                _dbContext.Accounts.Remove(delAccount);
                await _dbContext.SaveChangesAsync();
            }
            return delAccount;
        }

        /// <summary>
        /// Update DB account records for new balance.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newBalance"></param>
        /// <returns></returns>
        public async Task UpdateAccountBalance(int accountId, decimal newBalance)
        {
            Account account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (account != null)
            {
                account.Balance = newBalance;
                _dbContext.Accounts.Update(account);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
