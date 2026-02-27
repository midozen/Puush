namespace Puush.Contracts.Api.Responses;

public class RecentUpload : IPuushResponse
{
    public int Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Url { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public int ViewCount { get; init; }
    
    public string Serialize()
    {
        return $"{Id},{CreatedAt:MM-dd-yyyy},{Url},{FileName},{ViewCount}";
    }
}
