using System;

namespace CloudStorageSirius.CloudStorageFiles.Models
{
    public class AuthResponse // Clase que representa la respuesta de autenticación
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        AuthResponse(string token, DateTime expiration)
        {
            Token = token;
            Expiration = expiration;
        }
    }
}
