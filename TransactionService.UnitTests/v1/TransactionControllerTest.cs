using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionService.BLLs.Contracts;
using TransactionService.DTOs;
using TransactionService.Controllers.v1;
using TransactionService.BLLs;
using TransactionService.Repositories;

namespace TransactionService.UnitTests.v1
{
    public class TransactionControllerTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async void GetTransactions_Success(int? accountId)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TransactionController>>();
            var mockBll = new Mock<ITransactionsBll>();
            mockBll.Setup(bll => bll.GetTransactions(accountId)).ReturnsAsync(
                    new TransactionResponseDto
                    {
                        Data = new List<TransactionInfo>()
                        {
                            {new TransactionInfo
                            {
                                AccountId = 1,
                                TransactionType = "Deposit",
                                TransactionAmount = 1000.00M
                            }
                            }
                        },
                        ResponseCode = 200,
                        Message = "Success"
                    }
                );
            var controller = new TransactionController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.GetTransactions(accountId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(99, 500)]
        public async void GetTransactions_Failure(int? accountId, int expectedStatusCode)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TransactionController>>();
            var mockBll = new Mock<ITransactionsBll>();
            mockBll.Setup(bll => bll.GetTransactions(accountId)).ReturnsAsync(
                    new TransactionResponseDto
                    {
                        ResponseCode = expectedStatusCode,
                        Message = "Error"
                    }
                );
            var controller = new TransactionController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.GetTransactions(accountId);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, expectedStatusCode);
        }

        [Fact]
        public async void PostDeposit_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TransactionController>>();
            var mockBll = new Mock<ITransactionsBll>();
            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 1,
                TransactionAmount = 500
            };
            mockBll.Setup(bll => bll.Deposit(mockDto)).ReturnsAsync(
                    new ResponseDto("Success", 200)
                );
            var controller = new TransactionController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.PostDeposit(mockDto);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(99, 500)]
        [InlineData(1, 403)]
        public async void PostDeposit_Failure(int accountId, int expectedStatusCode)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TransactionController>>();
            var mockBll = new Mock<ITransactionsBll>();
            TransactionDto mockDto = new TransactionDto
            {
                AccountId = accountId,
                TransactionAmount = 15000
            };
            mockBll.Setup(bll => bll.Deposit(mockDto)).ReturnsAsync(
                    new ResponseDto("Error", expectedStatusCode)
                );
            var controller = new TransactionController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.PostDeposit(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, expectedStatusCode);
        }

        [Fact]
        public async void PostWithdraw_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TransactionController>>();
            var mockBll = new Mock<ITransactionsBll>();
            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 1,
                TransactionAmount = 500
            };
            mockBll.Setup(bll => bll.Withdraw(mockDto)).ReturnsAsync(
                    new ResponseDto("Success", 200)
                );
            var controller = new TransactionController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.PostWithdraw(mockDto);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(99, 500)]
        [InlineData(1, 403)]
        public async void PostWithdraw_Failure(int accountId, int expectedStatusCode)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TransactionController>>();
            var mockBll = new Mock<ITransactionsBll>();
            TransactionDto mockDto = new TransactionDto
            {
                AccountId = accountId,
                TransactionAmount = 20000
            };
            mockBll.Setup(bll => bll.Withdraw(mockDto)).ReturnsAsync(
                    new ResponseDto("Error", expectedStatusCode)
                );
            var controller = new TransactionController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.PostWithdraw(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, expectedStatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async void GetTransactions_SuccessWithData(int? accountId)
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());

            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, TestHelper.GetMockAccountServiceProxy().Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.GetTransactions(accountId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotEmpty((((OkObjectResult)result).Value as TransactionResponseDto).Data);
        }

        [Fact]
        public async void GetTransactions_SuccessWithoutData()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());

            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, TestHelper.GetMockAccountServiceProxy().Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.GetTransactions(99);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Null((((OkObjectResult)result).Value as TransactionResponseDto).Data);
        }

        [Fact]
        public async void PostDeposit_Success_Full()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(3, 40000)).ReturnsAsync(
                new ResponseDto("Success", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(3)).ReturnsAsync(
                new AccountResponseDto
                {
                    Data = new List<AccountInfo>
                    {
                        new AccountInfo
                        {
                        AccountId = 3,
                        Balance = 36000.00M
                        }
                    },
                    ResponseCode = 200,
                    Message = "Success"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 3,
                TransactionAmount = 4000
            };

            //Act
            var result = await controller.PostDeposit(mockDto);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void PostDeposit_BadRequest_InvalidAmount()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(3, 36000)).ReturnsAsync(
                new ResponseDto("Success", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(3)).ReturnsAsync(
                new AccountResponseDto
                {
                    Data = new List<AccountInfo>
                    {
                        new AccountInfo
                        {
                        AccountId = 3,
                        Balance = 36000.00M
                        }
                    },
                    ResponseCode = 200,
                    Message = "Success"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 3,
                TransactionAmount = -1
            };

            //Act
            var result = await controller.PostDeposit(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 400);
        }

        [Fact]
        public async void PostDeposit_Forbidden_AmountExceedLimit()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(3, 56000)).ReturnsAsync(
                new ResponseDto("Error", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(3)).ReturnsAsync(
                new AccountResponseDto
                {
                    Data = new List<AccountInfo>
                    {
                        new AccountInfo
                        {
                        AccountId = 3,
                        Balance = 36000.00M
                        }
                    },
                    ResponseCode = 200,
                    Message = "Success"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 3,
                TransactionAmount = 20000
            };

            //Act
            var result = await controller.PostDeposit(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 403);
        }

        [Fact]
        public async void PostDeposit_Failure_AccountNotFound()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(99, 36000)).ReturnsAsync(
                new ResponseDto("Success", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(99)).ReturnsAsync(
                new AccountResponseDto
                {
                    ResponseCode = 404,
                    Message = "ERROR"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 99,
                TransactionAmount = 5000
            };

            //Act
            var result = await controller.PostDeposit(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 500);
        }

        [Fact]
        public async void PostWithdraw_Success_Full()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(3, 30000)).ReturnsAsync(
                new ResponseDto("Success", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(3)).ReturnsAsync(
                new AccountResponseDto
                {
                    Data = new List<AccountInfo>
                    {
                        new AccountInfo
                        {
                        AccountId = 3,
                        Balance = 36000.00M
                        }
                    },
                    ResponseCode = 200,
                    Message = "Success"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 3,
                TransactionAmount = 6000
            };

            //Act
            var result = await controller.PostWithdraw(mockDto);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void PostWithdraw_BadRequest_InvalidAmount()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(3, 30000)).ReturnsAsync(
                new ResponseDto("Success", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(3)).ReturnsAsync(
                new AccountResponseDto
                {
                    Data = new List<AccountInfo>
                    {
                        new AccountInfo
                        {
                        AccountId = 3,
                        Balance = 36000.00M
                        }
                    },
                    ResponseCode = 200,
                    Message = "Success"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 3,
                TransactionAmount = -1
            };

            //Act
            var result = await controller.PostWithdraw(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 400);
        }

        [Fact]
        public async void PostWithdraw_BadRequest_BalanceLimitExceed()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(3, 50)).ReturnsAsync(
                new ResponseDto("Success", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(3)).ReturnsAsync(
                new AccountResponseDto
                {
                    Data = new List<AccountInfo>
                    {
                        new AccountInfo
                        {
                        AccountId = 3,
                        Balance = 36000.00M
                        }
                    },
                    ResponseCode = 200,
                    Message = "Success"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 3,
                TransactionAmount = 35550
            };

            //Act
            var result = await controller.PostWithdraw(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 403);
        }

        [Fact]
        public async void PostWithdraw_BadRequest_BalancePercentageLimitExceed()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(3, 3000)).ReturnsAsync(
                new ResponseDto("Success", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(3)).ReturnsAsync(
                new AccountResponseDto
                {
                    Data = new List<AccountInfo>
                    {
                        new AccountInfo
                        {
                        AccountId = 3,
                        Balance = 36000.00M
                        }
                    },
                    ResponseCode = 200,
                    Message = "Success"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 3,
                TransactionAmount = 33000
            };

            //Act
            var result = await controller.PostWithdraw(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 403);
        }

        [Fact]
        public async void PostWithdraw_Failure_AccountNotFound()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<TransactionController>>();
            var mockBllLogger = new Mock<ILogger<TransactionsBll>>();
            var transactionsRepository = new TransactionsRepository(TestHelper.GetDbContext());
            var accountServiceProxy = TestHelper.GetMockAccountServiceProxy();
            accountServiceProxy.Setup(proxy => proxy.PutUpdateAccountBalance(99, 38000)).ReturnsAsync(
                new ResponseDto("Success", 200));
            accountServiceProxy.Setup(proxy => proxy.GetAccountById(3)).ReturnsAsync(
                new AccountResponseDto
                {
                    ResponseCode = 404,
                    Message = "Error"
                });
            var bll = new TransactionsBll(mockBllLogger.Object, transactionsRepository, accountServiceProxy.Object);
            var controller = new TransactionController(mockControllerLogger.Object, bll);

            TransactionDto mockDto = new TransactionDto
            {
                AccountId = 3,
                TransactionAmount = 33000
            };

            //Act
            var result = await controller.PostWithdraw(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 500);
        }
    }
}