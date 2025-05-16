namespace CloudStorageSirius.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using CloudStorageSirius.Services;



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

        // Endpoint para registrar usuarios (opcional)
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterUserAsync(request.Username, request.Password);

            if (!result)
                return BadRequest("No se pudo registrar el usuario");

            return Ok("Usuario registrado correctamente");
        }


    }
}
