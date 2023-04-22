using TransactionService.DTOs;

namespace TransactionService.ClientProxies.Contracts
{
    public interface IAccountServiceProxy
    {
        Task<AccountResponseDto> GetAccountById(int accountId);
        Task<ResponseDto> PutUpdateAccountBalance(int accountId, decimal newBalance);
    }
}
