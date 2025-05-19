namespace CloudStorageSirius.CloudStorageFiles.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using CloudStorageSirius.Services;
    using CloudStorageSirius.Models;



    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;

        public UserController(AuthService authService, UserService userService)
        {
            // Constructor de la clase UserController recibe objetos de la clase AuthService y UserService
            _authService = authService;
            _userService = userService;
        }
        //  Endpoint para login y generación de JWT
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (token == null)
                return Unauthorized("Credenciales incorrectas");

            return Ok(new { Token = token });
        }

        // Endpoint para registrar usuarios 
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterUserAsync(request.Username, request.Password);

            if (!result)
                return BadRequest("No se pudo registrar el usuario");

            return Ok("Usuario registrado correctamente");
        }

        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Usuario no autenticado");

            var user = await _userService.GetUserInfoAsync(username);
            if (user == null)
                return NotFound("Usuario no encontrado");

            return Ok(user);
        }

    }
}
