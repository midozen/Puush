using Puush.Contracts.Api.Enums;

namespace Puush.Contracts.Api.Responses;

public class UploadResponse : IPuushResponse
{
    public ResponseCode Code { get; set; }
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Usage { get; set; }
    
    public string Serialize()
    {
        return $"{(int)Code},{Url},{FileName},{Usage}";
    }
}
