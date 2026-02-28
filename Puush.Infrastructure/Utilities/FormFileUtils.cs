using Microsoft.AspNetCore.Http;

namespace Puush.Infrastructure.Utilities;

public static class FormFileUtils
{
    public static string GetFileExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
        return string.IsNullOrWhiteSpace(extension) ? "bin" : extension;
    }
}