using Microsoft.EntityFrameworkCore;

namespace Puush.Persistence.Models;

[Index(nameof(ShortCode), IsUnique = true)]
public class Upload
{
    public int Id { get; init; }
    public required string ShortCode { get; init; }
    public required string FileName { get; init; }
    public int ViewCount { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}