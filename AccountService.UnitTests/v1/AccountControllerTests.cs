using AccountService.BLLs;
using AccountService.BLLs.Contracts;
using AccountService.Controllers.v1;
using AccountService.DTOs;
using AccountService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace AccountService.UnitTests.v1
{
    public class AccountControllerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async void GetAccountByUserId_Success(int? userId)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            mockBll.Setup(bll => bll.GetAccounts(userId)).ReturnsAsync(
                    new AccountResponseDto
                    {
                        Data = new List<AccountInfo>()
                        {
                            {new AccountInfo
                            {
                                UserId = 1,
                                AccountId = 1,
                                Balance = 1000
                            }
                            }
                        },
                        ResponseCode = 200,
                        Message = "Success"
                    }
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.GetAccountByUserId(userId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(10, 500)]
        public async void GetAccountByUserId_Failure(int userId, int expectedStatusCode)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            mockBll.Setup(bll => bll.GetAccounts(userId)).ReturnsAsync(
                    new AccountResponseDto
                    {
                        ResponseCode = expectedStatusCode,
                        Message = "Error"
                    }
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.GetAccountByUserId(userId);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, expectedStatusCode);
        }

        [Fact]
        public async void GetAccountById_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            mockBll.Setup(bll => bll.GetAccountById(1)).ReturnsAsync(
                    new AccountResponseDto
                    {
                        Data = new List<AccountInfo>()
                        {
                            {new AccountInfo
                            {
                                UserId = 1,
                                AccountId = 1,
                                Balance = 1000
                            }
                            }
                        },
                        ResponseCode = 200,
                        Message = "Success"
                    }
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.GetAccountById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(404)]
        public async void GetAccountById_Failure(int expectedStatusCode)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            mockBll.Setup(bll => bll.GetAccountById(10)).ReturnsAsync(
                    new AccountResponseDto
                    {
                        ResponseCode = expectedStatusCode,
                        Message = "Error"
                    }
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.GetAccountById(10);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, expectedStatusCode);
        }

        [Fact]
        public async void PostCreateAccount_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            AccountDto mockDto = new AccountDto
            {
                UserId = 1,
                StartingBalance = 1000
            };
            mockBll.Setup(bll => bll.CreateAccount(mockDto)).ReturnsAsync(
                    new ResponseDto("Success", 200)
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.PostCreateAccount(mockDto);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(403)]
        public async void PostCreateAccount_Failure(int expectedStatusCode)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            AccountDto mockDto = new AccountDto
            {
                UserId = 199,
                StartingBalance = 50
            };
            mockBll.Setup(bll => bll.CreateAccount(mockDto)).ReturnsAsync(
                    new ResponseDto("Error", expectedStatusCode)
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.PostCreateAccount(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, expectedStatusCode);
        }

        [Fact]
        public async void DeleteAccount_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            mockBll.Setup(bll => bll.DeleteAccount(2)).ReturnsAsync(
                    new ResponseDto("Success", 200)
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.DeleteAccount(2);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(404)]
        public async void DeleteAccount_Failure(int expectedStatusCode)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            mockBll.Setup(bll => bll.DeleteAccount(99)).ReturnsAsync(
                    new ResponseDto("Error", expectedStatusCode)
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.DeleteAccount(99);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, expectedStatusCode);
        }

        [Fact]
        public async void PutUpdateAccount_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            mockBll.Setup(bll => bll.UpdateAccountBalance(1, 10000)).ReturnsAsync(
                    new ResponseDto("Success", 200)
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.PutUpdateAccount(1, 10000);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(403)]
        public async void PutUpdateAccount_Failure(int expectedStatusCode)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockBll = new Mock<IAccountBll>();
            mockBll.Setup(bll => bll.UpdateAccountBalance(99, 10000)).ReturnsAsync(
                    new ResponseDto("Error", expectedStatusCode)
                );
            var controller = new AccountController(mockLogger.Object, mockBll.Object);

            //Act
            var result = await controller.PutUpdateAccount(99, 10000);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, expectedStatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public async void GetAccountByUserId_SuccessWithData(int? userId)
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.GetAccountByUserId(userId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotEmpty((((OkObjectResult)result).Value as AccountResponseDto).Data);
        }

        [Fact]
        public async void GetAccountByUserId_SuccessWithoutData()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.GetAccountByUserId(999);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Null((((OkObjectResult)result).Value as AccountResponseDto).Data);
        }

        [Fact]
        public async void GetAccountById_SuccessWithData()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.GetAccountById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotEmpty((((OkObjectResult)result).Value as AccountResponseDto).Data);
        }

        [Fact]
        public async void GetAccountById_NotFound_AccountNotFound()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.GetAccountById(999);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 404);
        }

        [Fact]
        public async void PostCreateAccount_Success_Full()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            AccountDto mockDto = new AccountDto
            {
                UserId = 1,
                StartingBalance = 1000
            };

            //Act
            var result = await controller.PostCreateAccount(mockDto);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void PostCreateAccount_Forbidden_LowStartingBalance()
        {
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            AccountDto mockDto = new AccountDto
            {
                UserId = 1,
                StartingBalance = 50
            };

            //Act
            var result = await controller.PostCreateAccount(mockDto);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 403);
        }

        [Fact]
        public async void DeleteAccount_Success_Full()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.DeleteAccount(2);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteAccount_NotFound_AccountNotFound()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.DeleteAccount(99);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 404);
        }

        [Fact]
        public async void PutUpdateAccount_Success_Full()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.PutUpdateAccount(1, 15000);
            var accResult = await controller.GetAccountById(1);
            decimal newBalance = (((OkObjectResult)accResult).Value as AccountResponseDto).Data.FirstOrDefault().Balance;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(newBalance, 15000);
        }

        [Fact]
        public async void PutUpdateAccount_Forbidden_LowNewBalance()
        {
            // Arrange
            var mockControllerLogger = new Mock<ILogger<AccountController>>();
            var mockBllLogger = new Mock<ILogger<AccountBll>>();
            var usersRepository = new UsersRepository(TestHelper.GetDbContext());
            var accountsRepository = new AccountsRepository(TestHelper.GetDbContext());

            var bll = new AccountBll(mockBllLogger.Object, usersRepository, accountsRepository, TestHelper.GetMockTransactionServiceProxy().Object);
            var controller = new AccountController(mockControllerLogger.Object, bll);

            //Act
            var result = await controller.PutUpdateAccount(1, 50);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, 403);
        }
    }
}