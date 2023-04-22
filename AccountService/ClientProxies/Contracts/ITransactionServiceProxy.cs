using AccountService.DTOs;

namespace AccountService.ClientProxies.Contracts
{
    public interface ITransactionServiceProxy
    {
        Task<ResponseDto> PostDeposit(TransactionDto transaction);
    }
}
