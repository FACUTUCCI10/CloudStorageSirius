using System;

namespace CloudStorageSirius.CloudStorageFiles.Models
{
    public class FileMetaData // maneja los detalles de los archivos en la nube
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }

    }
}
