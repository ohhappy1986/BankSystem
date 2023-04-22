using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DbContexts.Contracts
{
    public interface IInMemoryDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Account> Accounts { get; set; }
    }
}
