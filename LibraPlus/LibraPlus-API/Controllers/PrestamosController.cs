using LibraPlus_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Entidades;

namespace LibraPlus_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly ProyectDBContext _context;

        public PrestamosController(ProyectDBContext context)
        {
            _context = context;
        }

        // GET: api/prestamos (Obtener todos los préstamos)
        [HttpGet]
        public async Task<ActionResult<List<PrestamosDTO>>> Get()
        {
            var prestamos = await _context.Prestamos.ToListAsync();

            var prestamosDTO = prestamos.Select(p => new PrestamosDTO
            {
                PrestamoID = p.PrestamoID,
                UsuarioID = p.UsuarioID,
                LibroID = p.LibroID,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                Devuelto = p.Devuelto
            }).ToList();

            return Ok(prestamosDTO);
        }

        // GET: api/prestamos/{id} (Obtener un préstamo por ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<PrestamosDTO>> Get(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
                return NotFound();

            var dto = new PrestamosDTO
            {
                PrestamoID = prestamo.PrestamoID,
                UsuarioID = prestamo.UsuarioID,
                LibroID = prestamo.LibroID,
                FechaInicio = prestamo.FechaInicio,
                FechaFin = prestamo.FechaFin,
                Devuelto = prestamo.Devuelto
            };

            return Ok(dto);
        }

        // POST: api/prestamos (Registrar un nuevo préstamo)
        [HttpPost]
        public async Task<ActionResult<PrestamosDTO>> Post([FromBody] PrestamosDTO nuevoPrestamo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validaciones adicionales

            if (nuevoPrestamo.UsuarioID <= 0)
                return BadRequest("UsuarioID debe ser un valor positivo.");

            if (nuevoPrestamo.LibroID <= 0)
                return BadRequest("LibroID debe ser un valor positivo.");

            if (nuevoPrestamo.FechaInicio == default)
                return BadRequest("FechaInicio es obligatoria y debe ser válida.");

            if (nuevoPrestamo.FechaFin != null && nuevoPrestamo.FechaFin < nuevoPrestamo.FechaInicio)
                return BadRequest("FechaFin no puede ser anterior a FechaInicio.");

            // Verificar si Usuario y Libro existen en la base de datos
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == nuevoPrestamo.UsuarioID);
            if (!usuarioExiste)
                return BadRequest("El usuario especificado no existe.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == nuevoPrestamo.LibroID);
            if (!libroExiste)
                return BadRequest("El libro especificado no existe.");

            var prestamo = new Prestamos
            {
                UsuarioID = nuevoPrestamo.UsuarioID,
                LibroID = nuevoPrestamo.LibroID,
                FechaInicio = nuevoPrestamo.FechaInicio,
                FechaFin = nuevoPrestamo.FechaFin,
                Devuelto = nuevoPrestamo.Devuelto
            };

            _context.Prestamos.Add(prestamo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al guardar el préstamo en la base de datos.");
            }

            nuevoPrestamo.PrestamoID = prestamo.PrestamoID;

            return CreatedAtAction(nameof(Get), new { id = prestamo.PrestamoID }, nuevoPrestamo);
        }

        // PUT: api/prestamos/{id} (Modificar un préstamo existente)
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PrestamosDTO prestamoModificado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != prestamoModificado.PrestamoID)
                return BadRequest("El ID en la URL debe coincidir con el ID del préstamo.");

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
                return NotFound();

            if (prestamoModificado.UsuarioID <= 0)
                return BadRequest("UsuarioID debe ser un valor positivo.");

            if (prestamoModificado.LibroID <= 0)
                return BadRequest("LibroID debe ser un valor positivo.");

            if (prestamoModificado.FechaInicio == default)
                return BadRequest("FechaInicio es obligatoria y debe ser válida.");

            if (prestamoModificado.FechaFin != null && prestamoModificado.FechaFin < prestamoModificado.FechaInicio)
            {
                return BadRequest("FechaFin no puede ser anterior a FechaInicio.");
            }

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == prestamoModificado.UsuarioID);
            if (!usuarioExiste)
                return BadRequest("El usuario especificado no existe.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == prestamoModificado.LibroID);
            if (!libroExiste)
                return BadRequest("El libro especificado no existe.");

            prestamo.UsuarioID = prestamoModificado.UsuarioID;
            prestamo.LibroID = prestamoModificado.LibroID;
            prestamo.FechaInicio = prestamoModificado.FechaInicio;
            prestamo.FechaFin = prestamoModificado.FechaFin;
            prestamo.Devuelto = prestamoModificado.Devuelto;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al actualizar el préstamo en la base de datos.");
            }

            return NoContent();
        }

        // DELETE: api/prestamos/{id} (Eliminar un préstamo)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
                return NotFound();

            _context.Prestamos.Remove(prestamo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al eliminar el préstamo en la base de datos.");
            }

            return NoContent();
        }
    }

}
