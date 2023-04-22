using Newtonsoft.Json;
using TransactionService.ClientProxies.Contracts;
using TransactionService.DTOs;

namespace TransactionService.ClientProxies
{
    /// <summary>
    /// Proxy for AccountService API call
    /// </summary>
    public class AccountServiceProxy : BaseRestfulProxy, IAccountServiceProxy
    {
        //hardcode accountServiceUrl for demo/test purpose
        //private readonly string _accountServiceUrl = "http://localhost:8100/api/account";

        private readonly ILogger<AccountServiceProxy> _logger;
        private readonly IConfiguration _configuration;

        public AccountServiceProxy(ILogger<AccountServiceProxy> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Call get account API with Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AccountResponseDto> GetAccountById(int accountId)
        {
            _logger.LogDebug("Making GetAccountById call.");
            string url = _configuration.GetSection("AccountServiceURL").Value + "/" + accountId;
            HttpResponseMessage response = await GetRequestAsync(url);
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                string rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AccountResponseDto>(rawJson);
            }
            return new AccountResponseDto
            {
                ResponseCode = (int)response.StatusCode,
                Message = response.ReasonPhrase
            };
        }

        /// <summary>
        /// Call update account balance API to update current balance.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newBalance"></param>
        /// <returns></returns>
        public async Task<ResponseDto> PutUpdateAccountBalance(int accountId, decimal newBalance)
        {
            _logger.LogDebug("Making PutUpdateAccountBalance call.");
            string url = _configuration.GetSection("AccountServiceURL").Value + "/" + accountId + "?newBalance=" + newBalance;
            HttpResponseMessage response = await PutRequestWithoutBodyAsync(url);
            return new ResponseDto(response.ReasonPhrase, (int)response.StatusCode);
        }
    }
}
