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

        var obj = await cdnService.GetFileAsync(upload.FileName);
        
        var contentType =
            (obj.Headers.ContentType == "application/octet-stream"
                ? GuessContentType(upload.FileName)
                : obj.Headers.ContentType)
            ?? "application/octet-stream";
        
        Response.Headers[HeaderNames.ContentDisposition] = "inline";
        
        return File(
            obj.ResponseStream, 
            contentType
        );
    }
    
    private static string? GuessContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".png"  => "image/png",
            ".jpg"  => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif"  => "image/gif",
            ".webp" => "image/webp",
            ".bmp"  => "image/bmp",
            ".svg"  => "image/svg+xml",
            ".ico"  => "image/x-icon",
            _ => null
        };
    }
}