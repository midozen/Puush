using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Puush.Persistence.Models;

[Index(nameof(ShortCode), IsUnique = true)]
public class Upload
{
    [Key] public long Id { get; init; }
    
    [MaxLength(10)] public required string ShortCode { get; init; }
    [MaxLength(256)] public required string FileName { get; init; }
    
    public long SizeBytes { get; init; }
    
    public int ViewCount { get; set; }
    
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    
    [ForeignKey(nameof(Account))]
    public required long AccountId { get; init; }
    public Account Account { get; init; } = null!;
}