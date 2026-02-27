using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Puush.Persistence.Models;

public class Session
{
    [Key]
    [MaxLength(16)]
    public required string Id { get; init; }

    [Required]
    public required byte[] SecretHash { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }

    [ForeignKey(nameof(Account))]
    public long AccountId { get; init; }
    public Account Account { get; init; } = null!;
}