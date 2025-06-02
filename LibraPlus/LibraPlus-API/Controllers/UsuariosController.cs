using LibraPlus_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<List<UsuariosDTO>>> GetAll()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        // GET: api/Usuarios/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuariosDTO>> GetById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(); // 404 si no existe

            return Ok(usuario);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuariosDTO>> Create([FromBody] UsuariosDTO nuevoUsuario)
        {
            // Asumimos que nuevoUsuario.UsuarioID == 0
            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            // Devuelve 201 Created con la URL del recurso recién creado
            return CreatedAtAction(nameof(GetById), new { id = nuevoUsuario.UsuarioID }, nuevoUsuario);
        }

        // PUT: api/Usuarios/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuariosDTO usuarioModificado)
        {
            if (id != usuarioModificado.UsuarioID)
                return BadRequest("El ID en la ruta debe coincidir con el UsuarioID del cuerpo.");

            var existente = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.UsuarioID == id);
            if (existente == null)
                return NotFound(); // 404 si no existe

            // Marcamos el DTO como modificado directamente
            _context.Entry(usuarioModificado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Usuarios.AnyAsync(u => u.UsuarioID == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent(); // 204 si se actualizó correctamente
        }

        // DELETE: api/Usuarios/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(); // 404 si no existe

            _context.Usuarios.Remove(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Si hay restricciones de clave foránea (DeleteBehavior.Restrict)
                return BadRequest("No se pudo eliminar el usuario. Puede tener registros relacionados.");
            }

            return NoContent(); // 204 si se eliminó correctamente
        }
    }
}
