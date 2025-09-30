using LibraPlus.Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministradorController : ControllerBase
    {
        private readonly ProyectDBContext _context;

        public AdministradorController(ProyectDBContext context)
        {
            _context = context;
        }

        // POST: api/administrador/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                return BadRequest(new { message = "Email y contraseña requeridos" });

            var admin = _context.Admins.FirstOrDefault(a => a.Email == loginDto.Email);

            if (admin != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.PasswordHash))
            {
                return Ok(new { message = "Login exitoso" });
            }

            return Unauthorized(new { message = "Credenciales incorrectas" });
        }
    }

    // DTO para login
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
