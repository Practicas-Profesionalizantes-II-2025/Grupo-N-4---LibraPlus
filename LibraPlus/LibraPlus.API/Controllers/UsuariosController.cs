using LibraPlus___Practica_Profesionalizante_II;
using LibraPlus.Aplicacion;
using Microsoft.AspNetCore.Mvc;
using LibraPlus.Aplicacion.DTOs;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarios _usuariosService;

        public UsuariosController(IUsuarios usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuariosService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuariosService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuariosDTO usuarioDto)
        {
            // 1. Validar el modelo para asegurarse de que los datos son correctos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var usuario = new Usuarios
                {
                    Nombre = usuarioDto.Nombre,
                    Email = usuarioDto.Email,
                    Reputación = usuarioDto.Reputación // ✨ Aquí está la corrección: asegura la asignación correcta.
                };

                await _usuariosService.AddAsync(usuario);
                return Ok(usuario); // Devuelve el objeto con su ID asignado
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuariosDTO usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var usuario = await _usuariosService.GetByIdAsync(id);
                if (usuario == null) return NotFound();

                usuario.Nombre = usuarioDto.Nombre;
                usuario.Email = usuarioDto.Email;
                usuario.Reputación = usuarioDto.Reputación; // ✨ Aquí está la corrección: asegura la asignación correcta.

                await _usuariosService.UpdateAsync(usuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuario = await _usuariosService.GetByIdAsync(id);
                if (usuario == null)
                {
                    // ✨ AHORA DEVUELVE NOTFOUND SI EL USUARIO NO EXISTE
                    return NotFound("El usuario no fue encontrado.");
                }

                await _usuariosService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
