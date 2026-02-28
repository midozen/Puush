using Microsoft.AspNetCore.Http;

namespace Puush.Infrastructure.Extensions;

public static class FormFileExtensions
{
    private static readonly HashSet<string> SupportedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        "png",
        "jpg",
        "jpeg",
        "gif",
        "webp",
        "bmp",
        "ico",
        "tif",
        "tiff",
        "heic",
        "heif"
    };

    private static readonly HashSet<string> SupportedImageContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/png",
        "image/jpeg",
        "image/jpg",
        "image/gif",
        "image/webp",
        "image/pjpeg",
        "image/bmp",
        "image/x-icon",
        "image/vnd.microsoft.icon",
        "image/tiff",
        "image/heic",
        "image/heif",
        "application/octet-stream"
    };
    
    extension(IFormFile file)
    {
        public string GetFileExtension()
        {
            var extension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
            return string.IsNullOrWhiteSpace(extension) ? "bin" : extension;
        }

        public bool IsSupportedImage()
        {
            var extension = file.GetFileExtension();
            if (!SupportedImageExtensions.Contains(extension))
                return false;

            if (string.IsNullOrWhiteSpace(file.ContentType))
                return false;

            return SupportedImageContentTypes.Contains(file.ContentType.Trim());
        }
    }
}