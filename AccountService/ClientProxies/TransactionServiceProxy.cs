using AccountService.ClientProxies.Contracts;
using AccountService.DTOs;

namespace AccountService.ClientProxies
{
    /// <summary>
    /// Proxy for communicate with TransactionService
    /// </summary>
    public class TransactionServiceProxy : BaseRestfulProxy, ITransactionServiceProxy
    {
        private readonly ILogger<TransactionServiceProxy> _logger;
        private readonly IConfiguration _configuration;

        public TransactionServiceProxy(ILogger<TransactionServiceProxy> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        /// <summary>
        /// Post Deposit API call to TransactionService
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<ResponseDto> PostDeposit(TransactionDto transaction)
        {
            _logger.LogDebug("Making PostDeposit call.");
            string url = _configuration.GetSection("TransactionServiceURL").Value + "/deposit";
            HttpResponseMessage response = await PostJsonRequestAsync(url, transaction);
            return new ResponseDto(response.ReasonPhrase, (int)response.StatusCode);
        }
    }
}
