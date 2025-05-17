namespace CloudStorageSirius.CloudStorageFiles.Models
{
    public class User // Clase que representa un usuario en el sistema
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Almacena la contraseña hasheada
    }
}
