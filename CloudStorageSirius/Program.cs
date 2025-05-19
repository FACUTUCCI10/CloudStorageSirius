using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//  1. Configuraci�n de JWT: Se obtiene la clave y los par�metros desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

//  2. Agregar autenticaci�n y validaci�n JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,   // Verifica el emisor del token
            ValidateAudience = true, // Verifica el destinatario del token
            ValidateLifetime = true, // Asegura que el token no haya expirado
            ValidateIssuerSigningKey = true, // Verifica la firma del token
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

//  3. Agregar autorizaci�n: Permite proteger endpoints con [Authorize]
builder.Services.AddAuthorization();

// 4. Registrar servicios esenciales de la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Permite detectar endpoints en Swagger
builder.Services.AddSwaggerGen(); // Agrega documentaci�n Swagger para probar la API

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//  5. Configurar el pipeline de solicitud HTTP
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseHttpsRedirection();                               
//app.UseAuthentication(); //  6. Habilitar autenticaci�n antes de autorizaci�n
//app.UseAuthorization();
//  7. Mapear controladores (endpoints de la API)
app.MapControllers();
app.Run();


