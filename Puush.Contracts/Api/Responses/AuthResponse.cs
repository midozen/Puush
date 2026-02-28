using Puush.Persistence.Models;
using Puush.Persistence.Models.Enums;

namespace Puush.Contracts.Api.Responses;

public class AuthResponse : IPuushResponse
{
    private AuthResponse(Account account, string apiKey, DateTimeOffset expires)
    {
        AccountType = account.AccountType;
        ApiKey = apiKey;
        ExpirationDate = expires; 
        Usage = account.UsageBytes;
    }
    
    public static AuthResponse FromAccount(Account account, string apiKey, DateTimeOffset expires)
        => new(account, apiKey, expires);
    
    private AccountType AccountType { get; }
    private string ApiKey { get; }
    private DateTimeOffset ExpirationDate { get; }
    private long Usage { get; }
    
    public string Serialize()
    {
        return $"{(int)AccountType},{ApiKey},{ExpirationDate:MM-dd-yyyy},{Usage}";
    }
}
