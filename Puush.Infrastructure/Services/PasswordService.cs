using Microsoft.AspNetCore.Identity;
using Puush.Persistence.Models;

namespace Puush.Infrastructure.Services;

public interface IPasswordService
{
    string HashPassword(Account account, string password);
    bool VerifyPassword(Account account, string password);
}

public class PasswordService(IPasswordHasher<Account> hasher) : IPasswordService
{
    public string HashPassword(Account account, string password)
        => hasher.HashPassword(account, password);

    public bool VerifyPassword(Account account, string password)
        => hasher.VerifyHashedPassword(account, account.PasswordHash, password)
            is PasswordVerificationResult.Success
            or PasswordVerificationResult.SuccessRehashNeeded;
}