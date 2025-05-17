namespace CloudStorageSirius.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string username, bool isAdmin)
    {
        // se establecen los valores del token obtenidos del archivo de configuración (appsettings.json)
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"]);

        var claims = new List<Claim>
        {

            new Claim(ClaimTypes.Name, username),
            new Claim("IsAdmin", isAdmin.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Se establece el tiempo de expiración del token
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
        // Devuelve el token generado
    }
    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        // valida las credenciales del usuario
        if (username == "admin" && password == "clave123")
        {
            return GenerateToken(username, isAdmin: true);
            //retorna el token si el usuario es admin y la contraseña es correcta

        }

        return null;
    }


}