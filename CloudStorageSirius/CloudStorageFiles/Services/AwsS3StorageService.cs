using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

public class AwsS3StorageService : ICloudStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public AwsS3StorageService(IAmazonS3 s3Client, string bucketName)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = "application/octet-stream"
        };

        await _s3Client.PutObjectAsync(putRequest);
        return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        var response = await _s3Client.GetObjectAsync(getRequest);
        return response.ResponseStream; 
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
        return true;
    }
    public async Task<long> GetUserStorageUsageAsync(string username)
    {
        var listObjectsRequest = new ListObjectsV2Request
        {
            BucketName = _bucketName
        };

        var response = await _s3Client.ListObjectsV2Async(listObjectsRequest);
        long totalSize = response.S3Objects.Sum(obj => obj.Size);

        return totalSize;
    }
}