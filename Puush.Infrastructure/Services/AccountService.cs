using Puush.Persistence;
using Puush.Persistence.Models;

namespace Puush.Infrastructure.Services;

public interface IAccountService
{
    Task<Account> CreateAccountAsync(string username, string password);
}

public class AccountService(DatabaseContext dbContext, IPasswordService passwordService) : IAccountService
{
    public async Task<Account> CreateAccountAsync(string username, string password)
    {
        var account = new Account { Username = username };

        account.PasswordHash = passwordService.HashPassword(account, password);

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync();
        return account;
    }
}