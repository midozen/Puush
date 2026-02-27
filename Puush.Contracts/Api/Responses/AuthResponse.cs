using Puush.Persistence.Models;
using Puush.Persistence.Models.Enums;

namespace Puush.Contracts.Api.Responses;

public class AuthResponse : IPuushResponse
{
    public AuthResponse() { }
    
    private AuthResponse(Account account, string apiKey, DateTimeOffset expires)
    {
        AccountType = account.AccountType;
        ApiKey = apiKey;
        ExpirationDate = expires;
        Usage = 0; // TODO: Calculate usage
    }
    
    public static AuthResponse FromAccount(Account account, string apiKey, DateTimeOffset expires)
        => new(account, apiKey, expires);
    
    public AccountType AccountType { get; init; }
    public string ApiKey { get; init; } = null!;
    public DateTimeOffset ExpirationDate { get; init; }
    public long Usage { get; init; }
    
    public string Serialize()
    {
        return $"{(int)AccountType},{ApiKey},{ExpirationDate:MM-dd-yyyy},{Usage}";
    }
}
