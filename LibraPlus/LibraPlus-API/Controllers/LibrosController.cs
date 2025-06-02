using LibraPlus_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Entidades;

namespace LibraPlus_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ProyectDBContext _context;

        public LibrosController(ProyectDBContext context)
        {
            _context = context;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<List<LibrosDTO>>> GetAll()
        {
            var libros = await _context.Libros.ToListAsync();
            return Ok(libros);
        }

        // GET: api/Libros/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibrosDTO>> GetById(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound(); // 404 si no existe

            return Ok(libro);
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult<LibrosDTO>> Create([FromBody] LibrosDTO nuevoLibro)
        {
            // Asumimos que nuevoLibro.LibroID == 0
            _context.Libros.Add(nuevoLibro);
            await _context.SaveChangesAsync();

            // Devuelve 201 Created con la URL del recurso recién creado
            return CreatedAtAction(nameof(GetById), new { id = nuevoLibro.LibroID }, nuevoLibro);
        }

        // PUT: api/Libros/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LibrosDTO libroModificado)
        {
            if (id != libroModificado.LibroID)
                return BadRequest("El ID en la ruta debe coincidir con el LibroID del cuerpo.");

            var existente = await _context.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.LibroID == id);
            if (existente == null)
                return NotFound(); // 404 si no existe

            // Marcamos el DTO como modificado directamente
            _context.Entry(libroModificado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si hubo un conflicto de concurrencia, verificamos que aún exista
                if (!await _context.Libros.AnyAsync(l => l.LibroID == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent(); // 204 si se actualizó correctamente
        }

        // DELETE: api/Libros/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound(); // 404 si no existe

            _context.Libros.Remove(libro);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Si hay restricciones de clave foránea (DeleteBehavior.Restrict)
                return BadRequest("No se pudo eliminar el libro. Puede tener registros relacionados.");
            }

            return NoContent(); // 204 si se eliminó correctamente
        }
    }
}
