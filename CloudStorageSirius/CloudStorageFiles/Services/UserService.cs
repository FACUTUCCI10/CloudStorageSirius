﻿namespace CloudStorageSirius.Services;

using System.Collections.Generic;

public class UserService
{
    private readonly Dictionary<string, (string PasswordHash, bool IsAdmin)> _users;

    public UserService()
    {
        //Usuarios de prueba en memoria (simulación de base de datos)
        _users = new Dictionary<string, (string, bool)>
        {
            { "admin", (BCrypt.Net.BCrypt.HashPassword("admin123"), true) }, //bcrypt es una librería para hashear contraseñas
            { "user", (BCrypt.Net.BCrypt.HashPassword("user123"), false) }
        };
    }

    public bool ValidateUser(string username, string password)
    {
        // Verifica si el usuario existe y si la contraseña es correcta
        if (_users.TryGetValue(username, out var user))
        {
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
        return false;
    }

    public (string Username, bool IsAdmin)? GetUser(string username)
    {   // Devuelve el usuario y su rol (admin o no admin)
        if (_users.TryGetValue(username, out var user))
        {
            return (username, user.IsAdmin);
        }
        return null;
    }
    public async Task<bool> RegisterUserAsync(string username, string password)
    {
        if (_users.ContainsKey(username))
            return false; // El usuario ya existe

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        _users[username] = (hashedPassword, false); // Se registra como no admin por defecto

        return true;
    }
    public async Task<(string Username, bool IsAdmin)?> GetUserInfoAsync(string username)
    {
        if (_users.TryGetValue(username, out var user))
        {
            return (username, user.IsAdmin);
        }
        return null;
    }
}