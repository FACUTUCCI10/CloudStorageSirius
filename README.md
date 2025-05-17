📁 Cloud File Storage API
Este proyecto es una API RESTful desarrollada en ASP.NET Core (.NET 8.0) que permite a los usuarios subir, descargar, eliminar y gestionar archivos en la nube, con soporte para múltiples proveedores de almacenamiento.Es una  simulación de un servicio de tipo Dropbox/Google Drive con failover y límites de uso.

✅ Funcionalidades principales
📦 Subida, descarga y eliminación de archivos en la nube.

☁️ Soporte para Azure Blob Storage como proveedor (extensible a otros como Amazon S3, MinIO, etc.).

🔐 Autenticación de usuarios mediante JWT (expira en 1 hora).

👤 Registro e inicio de sesión con roles (Admin, Usuario).

📊 Endpoint exclusivo para Admins con estadísticas de uso (/api/stats).

⛔ Límite de almacenamiento por usuario: 5 GB por mes.

🔁 Failover listo para ser extendido a múltiples servicios cloud.

🧪 API documentada y testeable desde Swagger (OpenAPI).

🐳 Listo para dockerizar (estructura preparada).


🔧 Tecnologías utilizadas

Lenguaje: C# (.NET 8.0)

Framework: ASP.NET Core Web API

Base de datos: SQL Server (mediante Entity Framework Core)

Autenticación: JWT (con expiración)

Cloud Storage: Azure Blob Storage

Documentación: Swagger (OpenAPI)

Gestor de paquetes: NuGet

IDE: Visual Studio
