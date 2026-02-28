using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Puush.Persistence.Models.Enums;

namespace Puush.Persistence.Models;

[Index(nameof(Username), IsUnique = true)]
public class Account
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }
    
    [MaxLength(20)] public required string Username { get; set; }
    [MaxLength(256)] public string PasswordHash { get; set; } = null!;
    
    public long UsageBytes { get; set; }
    
    public AccountType AccountType { get; set; } = AccountType.Free;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<Upload> Uploads { get; set; } = new List<Upload>();
}