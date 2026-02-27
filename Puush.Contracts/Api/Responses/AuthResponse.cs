using Puush.Persistence.Models;
using Puush.Persistence.Models.Enums;

namespace Puush.Contracts.Api.Responses;

public class AuthResponse : IPuushResponse
{
    private AuthResponse(Account account, string apiKey, DateTimeOffset expires, long usage)
    {
        AccountType = account.AccountType;
        ApiKey = apiKey;
        ExpirationDate = expires;
        Usage = usage; // TODO: Calculate usage
    }
    
    public static AuthResponse FromAccount(Account account, string apiKey, DateTimeOffset expires, long usage = 0)
        => new(account, apiKey, expires, usage);
    
    private AccountType AccountType { get; }
    private string ApiKey { get; }
    private DateTimeOffset ExpirationDate { get; }
    private long Usage { get; }
    
    public string Serialize()
    {
        return $"{(int)AccountType},{ApiKey},{ExpirationDate:MM-dd-yyyy},{Usage}";
    }
}
