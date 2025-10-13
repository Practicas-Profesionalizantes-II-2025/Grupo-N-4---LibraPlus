using LibraPlus.Aplicacion;
using LibraPlus.Aplicacion.DTOs;
using LibraPlus___Practica_Profesionalizante_II;
using LibraPlus.Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarios _usuariosService;
        private readonly ProyectDBContext _context;

        public UsuariosController(IUsuarios usuariosService, ProyectDBContext context)
        {
            _usuariosService = usuariosService;
            _context = context;
        }

        // ✅ 1. Obtener todos los usuarios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuariosService.GetAllAsync();
            return Ok(usuarios);
        }

        // ✅ 2. Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuariosService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");
            return Ok(usuario);
        }

        // ✅ 3. Crear usuario normal
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuariosDTO usuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuario = new Usuarios
                {
                    Nombre = usuarioDto.Nombre,
                    Email = usuarioDto.Email,
                    Password = usuarioDto.Password, // 👈 agregado
                    Reputacion = usuarioDto.Reputación
                };

                await _usuariosService.AddAsync(usuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // ✅ 4. Actualizar usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuariosDTO usuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuario = await _usuariosService.GetByIdAsync(id);
                if (usuario == null)
                    return NotFound("Usuario no encontrado.");

                usuario.Nombre = usuarioDto.Nombre;
                usuario.Email = usuarioDto.Email;
                usuario.Password = usuarioDto.Password; 
                usuario.Reputacion = usuarioDto.Reputación;

                await _usuariosService.UpdateAsync(usuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // ✅ 5. Eliminar usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuario = await _usuariosService.GetByIdAsync(id);
                if (usuario == null)
                    return NotFound("El usuario no fue encontrado.");

                await _usuariosService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // ✅ 6. Crear usuario de prueba (Mateo Ferrero)
        [HttpPost("crear-temporal")]
        public async Task<IActionResult> CrearTemporal()
        {
            var usuario = new Usuarios
            {
                Nombre = "Mateo Ferrero",
                Email = "mateomferrero@gmail.com",
                Password = "Mateo1234",
                Reputacion = 5
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        // ✅ 7. Endpoint de login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            if (user == null)
                return Unauthorized("Credenciales inválidas");

            return Ok(new
            {
                UsuarioId = user.UsuarioID,
                user.Nombre,
                user.Email,
                Reputacion = user.Reputacion
            });
        }
        // ✅ 8. Registrar usuario (desde el formulario del login)
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuariosDTO usuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existe = await _context.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email);
            if (existe)
                return BadRequest("Ya existe un usuario con ese email.");

            var nuevoUsuario = new Usuarios
            {
                Nombre = usuarioDto.Nombre,
                Email = usuarioDto.Email,
                Password = usuarioDto.Password,
                Reputacion = 0
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario registrado correctamente",
                nuevoUsuario.UsuarioID,
                nuevoUsuario.Email
            });
        }

    }
}
