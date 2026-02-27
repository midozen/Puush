using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;

namespace Puush.Infrastructure.Services;

public interface ICdnService
{
    Task<string> UploadFileAsync(string key, Stream fileStream, string contentType);
    string GetFileUrl(string key);
}

public sealed class CdnService : ICdnService
{
    private static string _bucketName = null!;
    private readonly IAmazonS3 _s3Client;

    public CdnService(IConfiguration configuration)
    {
        _bucketName = configuration["R2:BucketName"] ?? throw new InvalidOperationException("Missing R2:BucketName");
        
        var accountId = configuration["R2:AccountId"] ?? throw new InvalidOperationException("Missing R2:AccountId");
        var accessKey = configuration["R2:AccessKey"] ?? throw new InvalidOperationException("Missing R2:AccessKey");
        var secretKey = configuration["R2:SecretKey"] ?? throw new InvalidOperationException("Missing R2:SecretKey");

        var credentials = new BasicAWSCredentials(accessKey, secretKey);

        _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
            ForcePathStyle = true,
            AuthenticationRegion = "auto"
        });
    }

    public async Task<string> UploadFileAsync(string key, Stream fileStream, string contentType)
    {
        throw new NotImplementedException();
    }

    public string GetFileUrl(string key)
    {
        throw new NotImplementedException();
    }
}