using AccountService.Models;

namespace AccountService.Repositories.Contracts
{
    public interface IAccountsRepository
    {
        Task<List<Account>> GetAccounts();
        Task<List<Account>> GetAccountsByUserId(int userId);
        Task<Account> GetAccountById(int accountId);
        Task<int> CreateAccount(Account account);
        Task<Account> DeleteAccount(int accountId);
        Task UpdateAccountBalance(int accountId, decimal newBalance);
    }
}
