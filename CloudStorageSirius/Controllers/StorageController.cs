namespace CloudStorageSirius.CloudStorageFiles.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using CloudStorageSirius.Services;
    using CloudStorageSirius.CloudStorageFiles.Services;
    using System.Threading.Tasks;

    // Define el prefijo de la ruta para los endpoints de este controlador (ejemplo: /api/storage)
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔒 Requiere autenticación JWT para acceder a los endpoints

    public class StorageController : ControllerBase
    {
        private readonly StorageServices _storageService; // Servicio de almacenamiento que maneja la lógica de subir, descargar y eliminar archivos

        // Constructor que inyecta la dependencia del servicio de almacenamiento
        public StorageController(StorageServices storageService)
        {
            _storageService = storageService;
        }

        /// <summary>
        /// 📤 Sube un archivo al sistema de almacenamiento.
        /// </summary>
        /// <param name="file*">Archivo recibido desde la solicitud HTTP.</param>
        /// <returns>Devuelve la URL del archivo subido.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // si el archivo es nulo o vacío, retorna un error 400 (BadRequest)
            if (file == null || file.Length == 0)
                return BadRequest("Archivo inválido");

            // Llama al servicio de almacenamiento para subir el archivo
            var url = await _storageService.UploadFileAsync(file);

            // Retorna la URL generada del archivo subido
            return Ok(new { Url = url });
        }

        /// <summary>
        /// 📥 Descarga un archivo desde el sistema de almacenamiento.
        /// </summary>
        /// <param name="fileName">Nombre del archivo a descargar.</param>
        /// <returns>Devuelve el archivo como un `Stream`.</returns>
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            // Obtiene el archivo desde el sistema de almacenamiento
            var fileStream = await _storageService.DownloadFileAsync(fileName);

            // Si el archivo no existe, retorna un error 404 (NotFound)
            if (fileStream == null)
                return NotFound("Archivo no encontrado");

            //Retorna el archivo con su contenido
            return File(fileStream, "application/octet-stream", fileName);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="fileName">Nombre del archivo a eliminar.</param>
        /// <returns>Mensaje confirmando eliminación.</returns>
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            // Llama al servicio de almacenamiento para eliminar el archivo
            var result = await _storageService.DeleteFileAsync(fileName);

            //Si el archivo no se puede eliminar, retorna un error 404 (NotFound)
            if (!result)
                return NotFound("No se pudo eliminar el archivo");

            //Retorna un mensaje confirmando la eliminación
            return Ok(new { Message = "Archivo eliminado correctamente" });
        }
    }
}