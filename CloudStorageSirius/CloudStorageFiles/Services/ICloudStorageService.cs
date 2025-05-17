using System;
using System.IO;
using System.Threading.Tasks;

public interface ICloudStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task<Stream?> DownloadFileAsync(string fileName); 
    Task<bool> DeleteFileAsync(string fileName); 
    Task<long> GetUserStorageUsageAsync(string username); // maneja el uso de almacenamiento del usuario
}


