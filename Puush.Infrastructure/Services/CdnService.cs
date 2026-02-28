using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace Puush.Infrastructure.Services;

public interface ICdnService
{
    Task<string> UploadFileAsync(string key, Stream fileStream, string contentType);
    Task<GetObjectResponse> GetFileAsync(string key);
    Task DeleteFileAsync(string key);
    
    Task<GetObjectResponse> GetFileOrFallbackAsync(string key);
}

public sealed class CdnService : ICdnService
{
    private readonly string _bucketName;
    private readonly AmazonS3Client _s3Client;

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
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be empty.", nameof(key));

        ArgumentNullException.ThrowIfNull(fileStream);

        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Content type cannot be empty.", nameof(contentType));

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            AutoCloseStream = false,
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true
        };

        await _s3Client.PutObjectAsync(request);
        return key;
    }

    // TODO: Consider returning something that abstracts the S3 response instead of exposing the entire GetObjectResponse.
    public async Task<GetObjectResponse> GetFileAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be empty.", nameof(key));
        
        var response = await _s3Client.GetObjectAsync(_bucketName, key);

        return response;
    }
    
    public async Task DeleteFileAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be empty.", nameof(key));
        
        await _s3Client.DeleteObjectAsync(_bucketName, key);
    }

    public async Task<GetObjectResponse> GetFileOrFallbackAsync(string key)
    {
        try
        {
            var response = await GetFileAsync(key);
            
            return response;
        }
        catch (AmazonS3Exception ex) when ((int)ex.StatusCode == 404)
        {
            return await GetFileAsync("offline_thumb.jpg");
        }
    }
}
