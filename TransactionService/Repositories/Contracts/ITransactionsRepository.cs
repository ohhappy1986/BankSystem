using TransactionService.Models;

namespace TransactionService.Repositories.Contracts
{
    public interface ITransactionsRepository
    {
        Task<List<Transaction>> GetTransactions();
        Task<List<Transaction>> GetTransactionsByAccountId(int accountId);
        Task<int> RecordTransaction(Transaction transaction);
    }
}
