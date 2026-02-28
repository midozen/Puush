using Microsoft.AspNetCore.Http;

namespace Puush.Infrastructure.Utilities;

public static class FormFileUtils
{
    public static string GetNormalizedImageContentType(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            "png" => "image/png",
            "jpg" or "jpeg" => "image/jpeg",
            "gif" => "image/gif",
            "webp" => "image/webp",
            "bmp" => "image/bmp",
            "ico" => "image/x-icon",
            "tif" or "tiff" => "image/tiff",
            "heic" => "image/heic",
            "heif" => "image/heif",
            _ => "application/octet-stream"
        };
    }
}