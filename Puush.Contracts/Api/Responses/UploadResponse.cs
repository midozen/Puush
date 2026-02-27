using Puush.Contracts.Api.Responses.Enums;

namespace Puush.Contracts.Api.Responses;

public class UploadResponse : PuushResponse
{
    public ResponseCode Code { get; set; }
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Usage { get; set; }
    
    public override string Serialize()
    {
        return $"{(int)Code},{Url},{FileName},{Usage}";
    }
}
