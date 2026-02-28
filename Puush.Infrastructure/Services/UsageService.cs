using Microsoft.EntityFrameworkCore;
using Puush.Persistence;

namespace Puush.Infrastructure.Services;

public interface IUsageService
{
    Task<long> GetUsageAsync(long accountId);
    Task AddUsageAsync(long accountId, long sizeBytes);
    Task RemoveUsageAsync(long accountId, long sizeBytes);
}

public class UsageService(DatabaseContext dbContext) : IUsageService
{
    public async Task<long> GetUsageAsync(long accountId)
    {
        return await dbContext.Accounts
            .Where(a => a.Id == accountId)
            .Select(a => a.UsageBytes)
            .FirstOrDefaultAsync();
    }
    
    public async Task AddUsageAsync(long accountId, long sizeBytes)
    {
        await dbContext.Accounts
            .Where(a => a.Id == accountId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(a => a.UsageBytes, a => a.UsageBytes + sizeBytes));
    }
    
    public async Task RemoveUsageAsync(long accountId, long sizeBytes)
    {
        await dbContext.Accounts
            .Where(a => a.Id == accountId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(
                    a => a.UsageBytes,
                    a => a.UsageBytes > sizeBytes
                        ? a.UsageBytes - sizeBytes
                        : 0
                ));
    }
}