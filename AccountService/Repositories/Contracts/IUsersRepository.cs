using AccountService.Models;

namespace AccountService.Repositories.Contracts
{
    /// <summary>
    /// Users table repository
    /// </summary>
    public interface IUsersRepository
    {
        Task<User> GetUserById(int userId);
    }
}
