using TransactionService.DTOs;

namespace TransactionService.BLLs.Contracts
{
    public interface ITransactionsBll
    {
        Task<TransactionResponseDto> GetTransactions(int? accountId);
        Task<ResponseDto> Deposit(TransactionDto transaction);
        Task<ResponseDto> Withdraw(TransactionDto transaction);
    }
}
