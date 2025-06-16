using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraPlus_API.Data;
using Shared.DTO;
using Shared.Entidades;

namespace LibraPlus_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ProyectDBContext _context;

        public UsuariosController(ProyectDBContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<List<UsuariosDTO>>> Get()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            var usuariosDto = usuarios.Select(u => new UsuariosDTO
            {
                UsuarioID = u.UsuarioID,
                Nombre = u.Nombre,
                Email = u.Email,
                Reputacion = u.Reputación
            }).ToList();

            return Ok(usuariosDto);
        }

        // GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuariosDTO>> Get(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            var usuarioDto = new UsuariosDTO
            {
                UsuarioID = usuario.UsuarioID,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Reputacion = usuario.Reputación
            };

            return Ok(usuarioDto);
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<UsuariosDTO>> Post([FromBody] UsuariosDTO nuevoUsuarioDto)
        {
            if (string.IsNullOrWhiteSpace(nuevoUsuarioDto.Nombre))
                return BadRequest("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(nuevoUsuarioDto.Email) || !IsValidEmail(nuevoUsuarioDto.Email))
                return BadRequest("El email es obligatorio y debe tener un formato válido.");

            var usuarioExistente = await _context.Usuarios
                .AnyAsync(u => u.Email == nuevoUsuarioDto.Email);
            if (usuarioExistente)
                return Conflict("Ya existe un usuario con ese email.");

            var nuevoUsuario = new Usuarios
            {
                Nombre = nuevoUsuarioDto.Nombre.Trim(),
                Email = nuevoUsuarioDto.Email.Trim(),
                Reputación = nuevoUsuarioDto.Reputacion
            };

            _context.Usuarios.Add(nuevoUsuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al guardar el usuario.");
            }

            nuevoUsuarioDto.UsuarioID = nuevoUsuario.UsuarioID;

            return CreatedAtAction(nameof(Get), new { id = nuevoUsuario.UsuarioID }, nuevoUsuarioDto);
        }

        // PUT: api/usuarios/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UsuariosDTO usuarioModificadoDto)
        {
            if (id != usuarioModificadoDto.UsuarioID)
                return BadRequest("El ID de la URL no coincide con el del cuerpo de la solicitud.");

            if (string.IsNullOrWhiteSpace(usuarioModificadoDto.Nombre))
                return BadRequest("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(usuarioModificadoDto.Email) || !IsValidEmail(usuarioModificadoDto.Email))
                return BadRequest("El email es obligatorio y debe ser válido.");

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            var emailExistente = await _context.Usuarios
                .AnyAsync(u => u.Email == usuarioModificadoDto.Email && u.UsuarioID != id);
            if (emailExistente)
                return Conflict("Otro usuario ya tiene este email.");

            usuario.Nombre = usuarioModificadoDto.Nombre.Trim();
            usuario.Email = usuarioModificadoDto.Email.Trim();
            usuario.Reputación = usuarioModificadoDto.Reputacion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al actualizar el usuario.");
            }

            return NoContent();
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al eliminar el usuario.");
            }

            return NoContent();
        }

        // Validar formato de email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
