using AccountService.DTOs;

namespace AccountService.BLLs.Contracts
{
    public interface IAccountBll
    {
        Task<AccountResponseDto> GetAccounts(int? userId);
        Task<AccountResponseDto> GetAccountById(int accountId);
        Task<ResponseDto> CreateAccount(AccountDto newAccount);
        Task<ResponseDto> DeleteAccount(int accountId);
        Task<ResponseDto> UpdateAccountBalance(int accountId, decimal newBalance);
    }
}
