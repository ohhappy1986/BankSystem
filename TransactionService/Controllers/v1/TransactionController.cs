using Microsoft.AspNetCore.Mvc;
using TransactionService.BLLs.Contracts;
using TransactionService.DTOs;

namespace TransactionService.Controllers.v1
{
    /// <summary>
    /// Transaction Service Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionsBll _transactionsBll;
        public TransactionController(ILogger<TransactionController> logger, ITransactionsBll transactionsBll)
        {
            _logger = logger;
            _transactionsBll = transactionsBll;
        }

        /// <summary>
        /// API for get transacations from account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTransactions(int? accountId)
        {
            _logger.LogDebug("Enter Get API for Transaction");
            TransactionResponseDto response = await _transactionsBll.GetTransactions(accountId);
            if (response.ResponseCode != 200)
                return StatusCode(response.ResponseCode, response.Message);
            return Ok(response);
        }

        /// <summary>
        /// API for deposit transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deposit")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> PostDeposit([FromBody] TransactionDto transaction)
        {
            _logger.LogDebug("Enter PostDeposit API for Transaction");
            if (transaction == null)
                return BadRequest("Transaction cannot be null");
            ResponseDto response = await _transactionsBll.Deposit(transaction);
            if (response.ResponseCode != 200)
                return StatusCode(response.ResponseCode, response.Message);
            return Ok(response);
        }

        /// <summary>
        /// API for withdraw transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("withdraw")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> PostWithdraw([FromBody] TransactionDto transaction)
        {
            _logger.LogDebug("Enter PostWithdraw API for Transaction");
            if (transaction == null)
                return BadRequest("Transaction cannot be null");
            ResponseDto response = await _transactionsBll.Withdraw(transaction);
            if (response.ResponseCode != 200)
                return StatusCode(response.ResponseCode, response.Message);
            return Ok(response);
        }
    }
}