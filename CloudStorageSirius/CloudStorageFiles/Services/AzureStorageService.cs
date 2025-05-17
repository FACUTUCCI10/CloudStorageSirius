using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

public class AzureStorageService : ICloudStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public AzureStorageService(string connectionString, string containerName)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerName = containerName;
    }

    public async Task<string?> UploadFileAsync(Stream fileStream, string fileName, string username)
    {
        long storageUsed = await GetUserStorageUsageAsync(username);

        const long maxStorage = 5L * 1024 * 1024 * 1024; // 5GB en bytes

        if (storageUsed >= maxStorage)
        {
            throw new Exception("Has alcanzado el límite de almacenamiento de 5GB este mes.");
        }

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient($"{username}/{fileName}");

        await blobClient.UploadAsync(fileStream, true);
        return blobClient.Uri.ToString();
    }
    public async Task<Stream?> DownloadFileAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        var response = await blobClient.DownloadAsync();
        return response.Value.Content;
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<long> GetUserStorageUsageAsync(string username)
    {
        return 0;
    }
}
