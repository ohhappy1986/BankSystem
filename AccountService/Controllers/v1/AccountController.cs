using AccountService.BLLs.Contracts;
using AccountService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers.v1
{
    /// <summary>
    /// Account Service Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountBll _accountBll;

        public AccountController(
            ILogger<AccountController> logger,
            IAccountBll accountBll
            )
        {
            _logger = logger;
            _accountBll = accountBll;
        }

        /// <summary>
        /// API for Get accounts for an user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAccountByUserId(int? userId)
        {
            _logger.LogDebug("Enter GetAccountByUserId API for Account");
            AccountResponseDto response = await _accountBll.GetAccounts(userId);
            if (response.ResponseCode != 200)
                return StatusCode(response.ResponseCode, response.Message);
            return Ok(response);
        }

        /// <summary>
        /// API for Get account by account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{accountId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAccountById(int accountId)
        {
            _logger.LogDebug("Enter GetAccountById API for Account");
            AccountResponseDto response = await _accountBll.GetAccountById(accountId);
            if (response.ResponseCode != 200)
                return StatusCode(response.ResponseCode, response.Message);
            return Ok(response);
        }

        /// <summary>
        /// API for Create account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> PostCreateAccount([FromBody] AccountDto account)
        {
            _logger.LogDebug("Enter PostCreateAccount API for Account");
            if (account == null)
                return BadRequest("Account info cannot be empty.");
            ResponseDto response = await _accountBll.CreateAccount(account);
            if (response.ResponseCode != 200)
                return StatusCode(response.ResponseCode, response.Message);
            return Ok(response);
        }

        /// <summary>
        /// API for Delete account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{accountId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            _logger.LogDebug("Enter DeleteAccount API for Account");
            if (accountId <= 0)
                return BadRequest("Invalid account Id.");
            ResponseDto response = await _accountBll.DeleteAccount(accountId);
            if (response.ResponseCode != 200)
                return StatusCode(response.ResponseCode, response.Message);
            return Ok(response);
        }

        /// <summary>
        /// API for Update balance of the account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newBalance"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{accountId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> PutUpdateAccount(int accountId, decimal newBalance)
        {
            _logger.LogDebug("Enter PutUpdateAccount API for Account");
            ResponseDto response = await _accountBll.UpdateAccountBalance(accountId, newBalance);
            if (response.ResponseCode != 200)
                return StatusCode(response.ResponseCode, response.Message);
            return Ok(response);
        }
    }
}