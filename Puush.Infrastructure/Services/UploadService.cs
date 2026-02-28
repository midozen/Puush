using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Puush.Contracts.Api.Enums;
using Puush.Infrastructure.Utilities;
using Puush.Persistence;
using Puush.Persistence.Models;

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
        var extension = FormFileUtils.GetFileExtension(file);
        var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
        
        var shortCode = RandomUtils.GenerateSecureRandomString(4);
        var objectKey = $"{shortCode}.{extension}";

        await using var stream = file.OpenReadStream();
        await cdnService.UploadFileAsync(objectKey, stream, contentType);

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
        
        dbContext.Uploads.Remove(upload);
        await dbContext.SaveChangesAsync();
        
        await usageService.RemoveUsageAsync(accountId, upload.SizeBytes);
        
        return ResponseCode.Success;
    }
}