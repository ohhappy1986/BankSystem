using AccountService.DbContexts.Contracts;
using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DbContexts
{
    /// <summary>
    /// DbContext for In-Memory DB
    /// </summary>
    public class InMemoryDbContext : DbContext, IInMemoryDbContext
    {
        public InMemoryDbContext() { }
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
        {
            loadInitialData();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// For demo/test purpose only, populate sample data.
        /// </summary>
        private void loadInitialData()
        {
            //Load initial Users data for demo purpose.
            List<User> users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "user1",
                    Email = "user1@test.com"
                },
                new User
                {
                    Id = 2,
                    Username = "user2",
                    Email = "user2@test.com"
                }
            };
            this.Users.AddRange(users);

            //Load initial Accounts data for demo purpose.
            List<Account> accounts = new List<Account>
            {
                new Account
                {
                    Id = 1,
                    UserId = 1,
                    Balance = 15000.00M
                },
                new Account
                {
                    Id = 2,
                    UserId = 1,
                    Balance = 23000.00M
                },
                new Account
                {
                    Id = 3,
                    UserId = 2,
                    Balance = 36000.00M
                }
            };
            this.Accounts.AddRange(accounts);

            this.SaveChanges();
        }
    }
}
