using Microsoft.EntityFrameworkCore;
using Puush.Contracts.Api.Responses;
using Puush.Persistence;

namespace Puush.Infrastructure.Services;

public interface IAuthService
{
    Task<AuthResponse?> AuthenticateAsync(string username, string? password, string? apiKey);
}

public class AuthService(
    DatabaseContext dbContext,
    IPasswordService passwordService,
    ISessionService sessionService) : IAuthService
{
    public async Task<AuthResponse?> AuthenticateAsync(string username, string? password, string? apiKey)
    {
        username = username.Trim();
        apiKey = string.IsNullOrWhiteSpace(apiKey) ? null : apiKey.Trim();

        if (apiKey is not null)
            return await AuthenticateWithApiKeyAsync(apiKey);

        if (string.IsNullOrEmpty(password))
            return null;

        return await AuthenticateWithPasswordAsync(username, password);
    }

    private async Task<AuthResponse?> AuthenticateWithApiKeyAsync(string apiKey)
    {
        var session = await sessionService.ValidateSessionAsync(apiKey);
        if (session is null)
            return null;

        var account = await dbContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == session.AccountId);

        return account is null
            ? null
            : AuthResponse.FromAccount(account, apiKey, session.ExpiresAt);
    }

    private async Task<AuthResponse?> AuthenticateWithPasswordAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;

        var account = await dbContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Username == username);

        if (account is null)
            return null;

        if (!passwordService.VerifyPassword(account, password))
            return null;

        var (token, expires) = await sessionService.GenerateSessionAsync(account.Id);
        return AuthResponse.FromAccount(account, token, expires);
    }
}