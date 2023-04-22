using TransactionService.BLLs.Contracts;
using TransactionService.ClientProxies.Contracts;
using TransactionService.DTOs;
using TransactionService.Models;
using TransactionService.Repositories.Contracts;

namespace TransactionService.BLLs
{
    /// <summary>
    /// Business logic for TransactionService
    /// </summary>
    public class TransactionsBll : ITransactionsBll
    {
        private readonly ILogger<TransactionsBll> _logger;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IAccountServiceProxy _accountServiceProxy;
        public TransactionsBll(
            ILogger<TransactionsBll> logger,
            ITransactionsRepository transactionsRepository,
            IAccountServiceProxy accountServiceProxy
            )
        {
            _logger = logger;
            _transactionsRepository = transactionsRepository;
            _accountServiceProxy = accountServiceProxy;
        }

        /// <summary>
        /// Retrivei transactions list.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<TransactionResponseDto> GetTransactions(int? accountId)
        {
            _logger.LogDebug("Get Transactions API enter");
            List<Transaction> transactions = new List<Transaction>();
            TransactionResponseDto response = new TransactionResponseDto();
            try
            {
                if (accountId == null)
                    transactions = await _transactionsRepository.GetTransactions();
                else
                    transactions = await _transactionsRepository.GetTransactionsByAccountId((int)accountId);

                if (transactions != null && transactions.Count > 0)
                {
                    response.Data = new List<TransactionInfo>();
                    foreach (Transaction transaction in transactions)
                    {
                        response.Data.Add(new TransactionInfo
                        {
                            AccountId = transaction.AccountId,
                            TransactionType = transaction.TransactionType ? "Deposit" : "Withdraw",
                            TransactionAmount = transaction.TransactionAmount
                        });
                    }
                    response.ResponseCode = 200;
                    response.Message = "Success";
                }
                else
                {
                    response.ResponseCode = 200;
                    response.Message = "No Transactions Found.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Get Transactions API: {0}", ex);
                response.Data.Clear();
                response.ResponseCode = 500;
                response.Message = "Error in Get Transactions API: " + ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Perform a deposit transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<ResponseDto> Deposit(TransactionDto transaction)
        {
            _logger.LogDebug("Deposit Transaction API enter");
            try
            {
                AccountResponseDto response = await _accountServiceProxy.GetAccountById(transaction.AccountId);
                if (response.Data == null)
                    throw new Exception("Error in getting account info. Account has to be created first before making transaction.");
                AccountInfo account = response.Data.FirstOrDefault();

                if (transaction.TransactionAmount < 0)
                    return new ResponseDto("Invalid TransactionAmount.", 400);

                if (transaction.TransactionAmount > 10000.00M)
                    return new ResponseDto("Deposit more tha $10000 is not allowed.", 403);

                await _transactionsRepository.RecordTransaction(new Transaction
                {
                    AccountId = transaction.AccountId,
                    TransactionType = true,
                    TransactionAmount = transaction.TransactionAmount
                });

                await _accountServiceProxy.PutUpdateAccountBalance(transaction.AccountId, transaction.TransactionAmount + account.Balance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Deposit Transaction API: {0}", ex);
                return new ResponseDto("Error in Deposit Transaction API: " + ex.Message, 500);
            }
            return new ResponseDto("Success", 200);
        }

        /// <summary>
        /// Perform a withdraw transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<ResponseDto> Withdraw(TransactionDto transaction)
        {
            _logger.LogDebug("Withdraw Transaction API enter");
            try
            {
                AccountResponseDto response = await _accountServiceProxy.GetAccountById(transaction.AccountId);
                if (response.Data == null)
                    throw new Exception("Error in getting account info. Account has to be created first before making transaction.");
                AccountInfo account = response.Data.FirstOrDefault();

                if (transaction.TransactionAmount < 0)
                    return new ResponseDto("Invalid TransactionAmount.", 400);

                if (transaction.TransactionAmount > account.Balance * 0.9M)
                    return new ResponseDto("Withdraw more than 90% of the total balance at once is not allowed.", 403);

                if (account.Balance - transaction.TransactionAmount < 100M)
                    return new ResponseDto("Account balance will be lower than $100 after this transaction.", 403);

                await _transactionsRepository.RecordTransaction(new Transaction
                {
                    AccountId = transaction.AccountId,
                    TransactionType = false,
                    TransactionAmount = transaction.TransactionAmount
                });

                await _accountServiceProxy.PutUpdateAccountBalance(transaction.AccountId, account.Balance - transaction.TransactionAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Withdraw Transaction API: {0}", ex);
                return new ResponseDto("Error in Withdraw Transaction API: " + ex.Message, 500);
            }
            return new ResponseDto("Success", 200);
        }
    }
}
