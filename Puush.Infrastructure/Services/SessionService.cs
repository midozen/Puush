using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Puush.Infrastructure.Utilities;
using Puush.Persistence;
using Puush.Persistence.Models;

namespace Puush.Infrastructure.Services;

public interface ISessionService
{
    Task<(string token, DateTimeOffset expires)> GenerateSessionAsync(long accountId);
    Task<Session?> ValidateSessionAsync(string token);
}

public class SessionService(DatabaseContext dbContext) : ISessionService
{
    public async Task<(string token, DateTimeOffset expires)> GenerateSessionAsync(long accountId)
    {
        var now = DateTimeOffset.UtcNow;
        
        var id = RandomUtils.GenerateSecureRandomString();
        var secret = RandomUtils.GenerateSecureRandomString();
        var hashedSecret = HashSecret(secret);
        
        var session = new Session
        {
            Id = id,
            SecretHash = hashedSecret,
            CreatedAt = now,
            ExpiresAt = now.AddDays(30),
            AccountId = accountId
        };
        
        dbContext.Sessions.Add(session);
        await dbContext.SaveChangesAsync();
        
        return ($"{id}.{secret}", session.ExpiresAt);
    }

    public async Task<Session?> ValidateSessionAsync(string token)
    {
        var parts = token.Split('.', 2);
        if (parts.Length != 2) return null;
        
        var id = parts[0];
        var secret = parts[1];
        
        // 1. Check if session exists
        var session = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Id == id);
        if (session == null) return null;
        
        // 2. Check if session is expired
        if (session.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            await dbContext.Sessions.Where(s => s.Id == id).ExecuteDeleteAsync();
            return null;
        }

        // 3. Check if secret matches
        var hashedSecret = HashSecret(secret);

        if (!CryptographicOperations.FixedTimeEquals(hashedSecret, session.SecretHash))
            return null;

        return session;
    }

    private static byte[] HashSecret(string secret)
    {
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        return SHA256.HashData(secretBytes);
    }
}