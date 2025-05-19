
//using Azure.Storage.Blobs;
//using System;
//using System.IO;
//using System.Threading.Tasks;

//namespace CloudStorageSirius.CloudStorageFiles.Services
//{
//    public class AzureStorageService : ICloudStorageService
//    {
//        private readonly BlobServiceClient _blobServiceClient;
//        private readonly string _containerName;

//        public AzureStorageService(string connectionString, string containerName)
//        {
//            _blobServiceClient = new BlobServiceClient(connectionString);
//            _containerName = containerName;
//        }

//        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            await blobClient.UploadAsync(fileStream, true);
//            return blobClient.Uri.ToString();
//        }

//        public async Task<Stream?> DownloadFileAsync(string fileName)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            var response = await blobClient.DownloadAsync();
//            return response.Value.Content;
//        }

//        public async Task<bool> DeleteFileAsync(string fileName)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            return await blobClient.DeleteIfExistsAsync();
//        }

//        public async Task<long> GetUserStorageUsageAsync(string username)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
//            long totalSize = 0;

//            await foreach (var blobItem in containerClient.GetBlobsAsync())
//            {
//                if (blobItem.Name.StartsWith($"{username}/")) // Filtrar archivos del usuario
//                {
//                    totalSize += blobItem.Properties.ContentLength ?? 0;
//                }
//            }

//            return totalSize;
//        }
//    }
//}