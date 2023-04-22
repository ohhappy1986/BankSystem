using AccountService.DbContexts;
using AccountService.Models;
using AccountService.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repositories
{
    /// <summary>
    /// Users table repository
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly InMemoryDbContext _dbContext;
        public UsersRepository(InMemoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get an user by user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
