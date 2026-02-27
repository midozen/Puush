using Puush.Persistence.Models.Enums;

namespace Puush.Contracts.Api.Responses;

public class AuthResponse : PuushResponse
{
    public AccountType AccountType { get; init; }
    public required string ApiKey { get; init; }
    public DateTime ExpirationDate { get; init; }
    public long Usage { get; init; }
    
    public override string Serialize()
    {
        return $"{(int)AccountType},{ApiKey},{ExpirationDate:MM-dd-yyyy},{Usage}";
    }
}
