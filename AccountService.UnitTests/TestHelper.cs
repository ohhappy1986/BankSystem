using AccountService.ClientProxies.Contracts;
using AccountService.DbContexts;
using AccountService.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccountService.UnitTests
{
    public static class TestHelper
    {
        private static InMemoryDbContext dbContext;

        public static InMemoryDbContext GetDbContext()
        {
            DbContextOptionsBuilder<InMemoryDbContext> dbOptionsBuilder = new DbContextOptionsBuilder<InMemoryDbContext>();
            DbContextOptions<InMemoryDbContext> dbOptions = dbOptionsBuilder.UseInMemoryDatabase(databaseName: "testDb").Options;
            if (dbContext == null)
                dbContext = new InMemoryDbContext(dbOptions);
            return dbContext;
        }

        public static Mock<ITransactionServiceProxy> GetMockTransactionServiceProxy()
        {
            Mock<ITransactionServiceProxy> mockProxy = new Mock<ITransactionServiceProxy>();
            mockProxy.Setup(
            proxy => proxy.PostDeposit(new TransactionDto
            {
                AccountId = 1,
                TransactionAmount = 1000
            })).ReturnsAsync(new ResponseDto("Success", 200));
            return mockProxy;
        }
    }
}
