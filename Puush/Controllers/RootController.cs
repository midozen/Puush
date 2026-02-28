using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Puush.Infrastructure.Services;
using Puush.Persistence;

namespace Puush.Controllers;

[ApiController]
[Route("/")]
public class RootController(DatabaseContext dbContext, ICdnService cdnService) : ControllerBase
{
    [HttpGet("{shortCode}")]
    [HttpGet("{shortCode}/{*fileName}")] // FileName is ultimately useless.
    public async Task<IActionResult> GetUpload(string shortCode, string? fileName)
    {
        var upload = await dbContext.Uploads
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        
        if (upload is null)
            return NotFound();
        
        // Increment view count asynchronously without blocking the response
        await dbContext.Uploads
            .Where(u => u.ShortCode == shortCode)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(u => u.ViewCount, u => u.ViewCount + 1));

        var obj = await cdnService.GetFileOrFallbackAsync(upload.FileName);
        
        var contentType =
            (obj.Headers.ContentType == "application/octet-stream"
                ? GuessContentType(upload.FileName)
                : obj.Headers.ContentType)
            ?? "application/octet-stream";
        
        // Maybe FileName isn't useless.
        Response.Headers[HeaderNames.ContentDisposition] =
            string.IsNullOrWhiteSpace(fileName)
                ? "inline"
                : $"inline; filename=\"{fileName}\"";
        
        return File(
            obj.ResponseStream, 
            contentType
        );
    }
    
    [HttpGet("thumb/{shortCode}")]
    public async Task<IActionResult> GetThumb(string shortCode)
    {
        var key = $"{shortCode}_thumb.jpg";

        var file = await cdnService.GetFileOrFallbackAsync(key);
        
        return File(
            file.ResponseStream, 
            file.Headers.ContentType
        );
    }
    
    // TODO: use the util in infra instead of local method
    private static string? GuessContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        
        return ext switch
        {
            ".png" => "image/png",
            ".jpg" or "jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".bmp" => "image/bmp",
            ".ico" => "image/x-icon",
            ".tif" or "tiff" => "image/tiff",
            ".heic" => "image/heic",
            ".heif" => "image/heif",
            _ => "application/octet-stream"
        };
    }
}