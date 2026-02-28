using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Puush.Contracts.Api.Enums;
using Puush.Infrastructure.Extensions;
using Puush.Infrastructure.Utilities;
using Puush.Persistence;
using Puush.Persistence.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Puush.Infrastructure.Services;

public interface IUploadService
{
    Task<Upload> AddUploadAsync(IFormFile file, long accountId);
    Task<ResponseCode> DeleteUploadAsync(long id, long accountId);
}

public class UploadService(DatabaseContext dbContext, IUsageService usageService, ICdnService cdnService) : IUploadService
{
    public async Task<Upload> AddUploadAsync(IFormFile file, long accountId)
    {
        var extension = file.GetFileExtension();
        var contentType = FormFileUtils.GetNormalizedImageContentType(extension);
        
        var shortCode = await GenerateUniqueShortCodeAsync();
        var objectKey = $"{shortCode}.{extension}";

        await using (var uploadStream = file.OpenReadStream())
        {
            await cdnService.UploadFileAsync(objectKey, uploadStream, contentType);
        }

        await using (var thumbStream = file.OpenReadStream())
        {
            await TryCreateThumbnailAsync(thumbStream, shortCode);
        }
        
        var upload = new Upload
        {
            ShortCode = shortCode,
            FileName = objectKey,
            SizeBytes = file.Length,
            AccountId = accountId
        };

        dbContext.Uploads.Add(upload);
        await dbContext.SaveChangesAsync();
        
        await usageService.AddUsageAsync(accountId, file.Length);
        
        return upload;
    }

    public async Task<ResponseCode> DeleteUploadAsync(long id, long accountId)
    {
        var upload = await dbContext.Uploads
            .FirstOrDefaultAsync(u => u.Id == id && u.AccountId == accountId);
        if (upload is null)
            return ResponseCode.Unknown;
        
        await cdnService.DeleteFileAsync(upload.FileName);
        await TryDeleteObjectAsync($"{upload.ShortCode}_thumb.jpg");
        
        dbContext.Uploads.Remove(upload);
        await dbContext.SaveChangesAsync();
        
        await usageService.RemoveUsageAsync(accountId, upload.SizeBytes);
        
        return ResponseCode.Success;
    }

    private async Task TryCreateThumbnailAsync(Stream stream, string shortCode)
    {
        var thumbnailKey = $"{shortCode}_thumb.jpg";
        
        try
        {
            var thumbnailStream = await ImageProcessingUtils.CreateThumbnailAsync(stream);

            await cdnService.UploadFileAsync(thumbnailKey, thumbnailStream, "image/jpeg");
        }
        catch
        {
            // Ignore an
        }
    }

    private async Task TryDeleteObjectAsync(string key)
    {
        try
        {
            await cdnService.DeleteFileAsync(key);
        }
        catch (AmazonS3Exception ex) when ((int)ex.StatusCode == 404) { }
    }
    
    private async Task<string> GenerateUniqueShortCodeAsync()
    {
        const int maxAttempts = 10;

        for (var i = 0; i < maxAttempts; i++)
        {
            var candidate = RandomUtils.GenerateSecureRandomString(4);
            
            var exists = await dbContext.Uploads.AnyAsync(u => u.ShortCode == candidate);
            if (!exists)
                return candidate;
        }

        throw new InvalidOperationException("Failed to allocate a unique upload short code.");
    }
}
