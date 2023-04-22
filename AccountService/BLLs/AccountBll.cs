using AccountService.BLLs.Contracts;
using AccountService.ClientProxies.Contracts;
using AccountService.DTOs;
using AccountService.Models;
using AccountService.Repositories.Contracts;

namespace AccountService.BLLs
{
    /// <summary>
    /// Business Logic for AccountService
    /// </summary>
    public class AccountBll : IAccountBll
    {
        private readonly ILogger<AccountBll> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IAccountsRepository _accountsRepository;
        private readonly ITransactionServiceProxy _transactionServiceProxy;

        public AccountBll(
            ILogger<AccountBll> logger,
            IUsersRepository usersRepository,
            IAccountsRepository accountsRepository,
            ITransactionServiceProxy transactionServiceProxy
            )
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _accountsRepository = accountsRepository;
            _transactionServiceProxy = transactionServiceProxy;
        }

        /// <summary>
        /// Retrieve accounts info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AccountResponseDto> GetAccounts(int? userId)
        {
            _logger.LogDebug("Get Accounts API enter");
            List<Account> accounts = new List<Account>();
            AccountResponseDto response = new AccountResponseDto();
            try
            {
                if (userId == null)
                    accounts = await _accountsRepository.GetAccounts();
                else
                    accounts = await _accountsRepository.GetAccountsByUserId((int)userId);
                if (accounts != null && accounts.Count > 0)
                {
                    response.Data = new List<AccountInfo>();
                    foreach (Account account in accounts)
                    {
                        response.Data.Add(new AccountInfo
                        {
                            AccountId = account.Id,
                            UserId = account.UserId,
                            Balance = account.Balance
                        });
                    }
                    response.ResponseCode = 200;
                    response.Message = "Success";
                }
                else
                {
                    response.ResponseCode = 200;
                    response.Message = "No Account Found.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Get Accounts API: {0}", ex);
                response.Data.Clear();
                response.ResponseCode = 500;
                response.Message = "Error in Get Accounts API: " + ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Retrieve an account by account id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AccountResponseDto> GetAccountById(int accountId)
        {
            _logger.LogDebug("Get Accounts By Id API enter");
            AccountResponseDto response = new AccountResponseDto();
            try
            {
                Account account = await _accountsRepository.GetAccountById(accountId);
                if (account != null)
                {
                    response.Data = new List<AccountInfo>
                    {
                        new AccountInfo
                        {
                            AccountId = account.Id,
                            UserId = account.UserId,
                            Balance = account.Balance
                        }
                    };
                    response.ResponseCode = 200;
                    response.Message = "Success";
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Message = "Account Id not found.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Get Accounts By Id API: {0}", ex);
                response.Data.Clear();
                response.ResponseCode = 500;
                response.Message = "Error in Get Accounts By Id API: " + ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Create account
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CreateAccount(AccountDto newAccount)
        {
            _logger.LogDebug("Create Account API enter");
            try
            {
                User user = await _usersRepository.GetUserById(newAccount.UserId);
                if (user == null)
                    return new ResponseDto("User has to be created first before create account.", 500);
                if (newAccount.StartingBalance < 100.00M)
                    return new ResponseDto("Initial deposit has to be higher than $100.", 403);

                int newAccountId = await _accountsRepository.CreateAccount(new Account
                {
                    UserId = newAccount.UserId,
                    Balance = 0.00M
                });

                await _transactionServiceProxy.PostDeposit(new TransactionDto
                {
                    AccountId = newAccountId,
                    TransactionAmount = newAccount.StartingBalance
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Create Account API: {0}", ex);
                return new ResponseDto("Error in Create Account API: " + ex.Message, 500);
            }
            return new ResponseDto("Success", 200);
        }

        /// <summary>
        /// Delete account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseDto> DeleteAccount(int accountId)
        {
            _logger.LogDebug("Delete Account API enter");
            try
            {
                Account delAccount = await _accountsRepository.DeleteAccount(accountId);
                if (delAccount == null)
                {
                    return new ResponseDto("No Account to delete ", 404);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Delete Account API: {0}", ex);
                return new ResponseDto("Error in Delete Account API: " + ex.Message, 500);
            }
            return new ResponseDto("Success", 200);
        }

        /// <summary>
        /// Update account balance
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newBalance"></param>
        /// <returns></returns>
        public async Task<ResponseDto> UpdateAccountBalance(int accountId, decimal newBalance)
        {
            _logger.LogDebug("Update Account Balance API enter");
            try
            {
                if (newBalance < 100)
                {
                    return new ResponseDto("New Account balance should not be less than $100", 403);
                }
                await _accountsRepository.UpdateAccountBalance(accountId, newBalance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Update Account Balance API: {0}", ex);
                return new ResponseDto("Error in Update Account Balance API: " + ex.Message, 500);
            }
            return new ResponseDto("Success", 200);
        }
    }
}
