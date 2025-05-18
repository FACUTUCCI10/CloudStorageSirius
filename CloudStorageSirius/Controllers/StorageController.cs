namespace CloudStorageSirius.CloudStorageFiles.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using CloudStorageSirius.Services;
    using CloudStorageSirius.CloudStorageFiles.Services;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    // Todos los endpoints requieren autenticación
    public class StorageController:ControllerBase
    {
        private readonly StorageServices _storageService;

        public StorageController(StorageServices storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            
            if (file == null || file.Length == 0) return BadRequest("Archivo inválido");

            var url = await _storageService.UploadFileAsync(file);
            return Ok(new { Url = url });
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var fileStream = await _storageService.DownloadFileAsync(fileName);
            if (fileStream == null) return NotFound("Archivo no encontrado");

            return File(fileStream, "application/octet-stream", fileName);
        }

        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var result = await _storageService.DeleteFileAsync(fileName);
            if (!result) return NotFound("No se pudo eliminar el archivo");

            return Ok(new { Message = "Archivo eliminado correctamente" });
        }

    }
}
