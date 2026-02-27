using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Puush.Persistence.Models.Enums;

namespace Puush.Persistence.Models;

public class Session
{
    [Key]
    [MaxLength(16)]
    public required string Id { get; init; }

    public required byte[] SecretHash { get; init; }
    
    // TODO: Possibly add additional client information such as IP address, user agent, etc. for better session management and security.
    public SessionClientType ClientType { get; init; } = SessionClientType.Unknown;

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; set; }

    [ForeignKey(nameof(Account))]
    public long AccountId { get; init; }
    public Account Account { get; init; } = null!;
}