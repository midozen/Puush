using Puush.Contracts.Api.Enums;

namespace Puush.Contracts.Api.Responses;

public class UploadResponse : IPuushResponse
{
    public ResponseCode Code { get; init; }
    public string Url { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public long Usage { get; init; }
    
    public string Serialize()
    {
        return $"{(int)Code},{Url},{FileName},{Usage}";
    }
}
