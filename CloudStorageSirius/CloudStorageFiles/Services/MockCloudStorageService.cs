//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//public class MockCloudStorageService : ICloudStorageService
//{
//    // Simulación de almacenamiento en memoria
//    private readonly Dictionary<string, List<(string FileName, long FileSize)>> _userStorage = new();

//    public async Task<string?> UploadFileAsync(Stream fileStream, string fileName)
//    {
//        _storage[fileName] = new byte[fileStream.Length];
//        fileStream.Read(_storage[fileName], 0, (int)fileStream.Length);
//        return Task.FromResult($"mock://storage/{fileName}");

//    }

//    public async Task<Stream?> DownloadFileAsync(string fileName)
//    {
//        return new MemoryStream(); // Simulación vacía
//    }

//    public async Task<bool> DeleteFileAsync(string fileName)
//    {
//        foreach (var userFiles in _userStorage.Values)
//        {
//            var file = userFiles.Find(f => f.FileName == fileName);
//            if (file.FileName != null)
//            {
//                userFiles.Remove(file);
//                return true;
//            }
//        }
//        return false;
//    }

//    public async Task<long> GetUserStorageUsageAsync(string username)
//    {
//        return _userStorage.ContainsKey(username)
//            ? _userStorage[username].Sum(f => f.FileSize)
//            : 0;
//    }
//}

