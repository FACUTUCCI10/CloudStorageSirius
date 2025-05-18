using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Amazon.S3;


var builder = WebApplication.CreateBuilder(args);

//  1. Configuración de JWT desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

builder.Services.AddAuthorization();

//  2. Configurar el servicio de almacenamiento en AWS S3 o en modo Mock
var useMock = builder.Configuration.GetValue<bool>("UseMockStorage");
var awsConfig = builder.Configuration.GetSection("AWS");

if (useMock)
{
    builder.Services.AddSingleton<ICloudStorageService, MockCloudStorageService>();
}
else
{
    builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(
        awsConfig["AccessKey"],
        awsConfig["SecretKey"],
        Amazon.RegionEndpoint.GetBySystemName(awsConfig["Region"])
    ));

    builder.Services.AddSingleton<ICloudStorageService>(provider =>
        new AwsS3StorageService(
            provider.GetRequiredService<IAmazonS3>(),
            awsConfig["BucketName"]
        )
    );
}

//  3. Registrar servicios esenciales de la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//  4. Configurar el pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  5. Habilitar autenticación antes de autorización
app.UseAuthentication();
app.UseAuthorization();

//  6. Mapear controladores (endpoints de la API)
app.MapControllers();

//  7. Iniciar la aplicación
app.Run();