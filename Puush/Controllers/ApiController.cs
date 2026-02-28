using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Puush.Contracts.Api.Enums;
using Puush.Contracts.Api.Responses;
using Puush.Infrastructure.Extensions;
using Puush.Infrastructure.Security.Attributes;
using Puush.Infrastructure.Services;
using Puush.Persistence;
using Puush.Persistence.Models;
using Puush.Shared.Web;

namespace Puush.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController(
    DatabaseContext dbContext,
    IConfiguration configuration,
    IAuthService authService,
    ICdnService cdnService,
    IUploadService uploadService,
    IUsageService usageService) : PuushControllerBase
{
    [HttpPost("auth")]
    public async Task<IActionResult> Auth(
        [FromForm(Name = "e")] string username,
        [FromForm(Name = "p")] string? password,
        [FromForm(Name = "k")] string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey) && string.IsNullOrEmpty(password))
            return PuushCode(ResponseCode.AuthenticationFailure);

        var response = await authService.AuthenticateAsync(username, password, apiKey);
        return response is null ? PuushCode(ResponseCode.AuthenticationFailure) : Puush(response);
    }

    [PuushAuthorize]
    [HttpPost("hist")]
    public async Task<IActionResult> History()
    {
        var uploads = await dbContext.Uploads
            .Where(u => u.AccountId == AccountId)
            .OrderByDescending(u => u.CreatedAt)
            .Take(5)
            .Select(u => new RecentUpload
            {
                Id = u.Id,
                CreatedAt = u.CreatedAt,
                Url = $"{configuration["Puush:BaseUrl"]}/{u.ShortCode}",
                FileName = u.FileName,
                ViewCount = u.ViewCount
            })
            .ToListAsync();
        
        return PuushArray(ResponseCode.Success, uploads);
    }

    [PuushAuthorize]
    [HttpPost("thumb")]
    public async Task<IActionResult> Thumbnail([FromForm(Name = "i")] long id)
    {
        var shortCode = await dbContext.Uploads
            .AsNoTracking()
            .Where(u => u.Id == id && u.AccountId == AccountId)
            .Select(u => u.ShortCode)
            .FirstOrDefaultAsync();
        if (shortCode is null)
            return PuushCode(ResponseCode.Unknown);
        
        var key = $"{shortCode}_thumb.jpg";

        var file = await cdnService.GetFileOrFallbackAsync(key);
        
        return File(
            file.ResponseStream, 
            file.Headers.ContentType
        );
    }

    [PuushAuthorize]
    [HttpPost("del")]
    public async Task<IActionResult> DeleteUpload([FromForm(Name = "i")] long id)
    {
        if (AccountId is null)
            return Unauthorized();
        
        var result = await uploadService.DeleteUploadAsync(id, AccountId.Value);
        
        return PuushCode(result);
    }
    
    // TODO: Validate checksum of uploaded file and return error if it doesn't match
    // TODO: If this is a free account, check if user has exceeded their upload limit and return error if they have
    [PuushAuthorize]
    [HttpPost("up")]
    public async Task<IActionResult> UploadImage([FromForm(Name = "f")] IFormFile file)
    {
        if (AccountId is null)
            return Unauthorized();
        
        if (file.Length <= 0 || !file.IsSupportedImage())
            return PuushCode(ResponseCode.Unknown);

        var upload = await uploadService.AddUploadAsync(file, AccountId.Value);

        return Puush(new UploadResponse
        {
            Code = ResponseCode.Success,
            Url = $"{configuration["Puush:BaseUrl"]}/{upload.ShortCode}",
            FileName = upload.FileName,
            Usage = await usageService.GetUsageAsync(AccountId.Value)
        });
    }
}
