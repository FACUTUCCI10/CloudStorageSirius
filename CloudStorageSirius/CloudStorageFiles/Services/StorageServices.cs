using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace CloudStorageSirius.CloudStorageFiles.Services
{
    public class StorageServices
    {
        private readonly BlobContainerClient _containerClient;

        //Constructor para inicializar conexión con Azure Blob Storage
        public StorageServices(IConfiguration config)
        {
            var azureSettings = config.GetSection("AzureStorage");
            var connectionString = azureSettings["ConnectionString"];
            var containerName = azureSettings["ContainerName"];

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists(); //Crea el contenedor si no existe
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            return blobClient.Uri.ToString(); //Devuelve la URL del archivo
        }

        public async Task<Stream?> DownloadFileAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync()) return null;

            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
