using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Puush.Persistence.Models.Enums;

namespace Puush.Persistence.Models;

[Index(nameof(Username), IsUnique = true)]
public class Account
{
    [Key] public long Id { get; init; }
    [MaxLength(20)] public required string Username { get; init; }
    // public string Email { get; init; } (ADD THIS LATER IF NEED BE)
    [MaxLength(256)] public required string PasswordHash { get; init; }
    public AccountType AccountType { get; init; }
    public DateTime CreatedAt { get; init; }
}