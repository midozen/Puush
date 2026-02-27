namespace Puush.Contracts.Api.Responses;

public class RecentUpload : IPuushResponse
{
    public int Id { get; set; }
    public DateTime UploadDate { get; set; }
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public int ViewCount { get; set; }

    public string Serialize()
    {
        return $"{Id},{UploadDate:MM-dd-yyyy},{Url},{FileName},{ViewCount}";
    }
}
