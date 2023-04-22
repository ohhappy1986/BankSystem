using TransactionService.ClientProxies.Contracts;
using TransactionService.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TransactionService.UnitTests
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

        public static Mock<IAccountServiceProxy> GetMockAccountServiceProxy()
        {
            Mock<IAccountServiceProxy> mockProxy = new Mock<IAccountServiceProxy>();
            return mockProxy;
        }
    }
}
