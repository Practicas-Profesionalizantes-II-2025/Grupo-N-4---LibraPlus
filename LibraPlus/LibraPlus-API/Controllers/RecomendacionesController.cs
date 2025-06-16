using LibraPlus_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Entidades;

namespace LibraPlus_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecomendacionesController : ControllerBase
    {
        private readonly ProyectDBContext _context;

        public RecomendacionesController(ProyectDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecomendacionDTO>>> Get()
        {
            var recomendaciones = await _context.Recomendaciones.ToListAsync();

            var dtoList = recomendaciones.Select(r => new RecomendacionDTO
            {
                RecomendacionID = r.RecomendacionID,
                UsuarioID = r.UsuarioID,
                LibroID = r.LibroID,
                Fuente = r.Fuente
            }).ToList();

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecomendacionDTO>> Get(int id)
        {
            var recomendacion = await _context.Recomendaciones.FindAsync(id);
            if (recomendacion == null)
                return NotFound();

            var dto = new RecomendacionDTO
            {
                RecomendacionID = recomendacion.RecomendacionID,
                UsuarioID = recomendacion.UsuarioID,
                LibroID = recomendacion.LibroID,
                Fuente = recomendacion.Fuente
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<RecomendacionDTO>> Post([FromBody] RecomendacionDTO nuevaRecomendacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (nuevaRecomendacion.UsuarioID <= 0)
                return BadRequest("UsuarioID debe ser un valor positivo.");

            if (nuevaRecomendacion.LibroID <= 0)
                return BadRequest("LibroID debe ser un valor positivo.");

            if (string.IsNullOrWhiteSpace(nuevaRecomendacion.Fuente))
                return BadRequest("La fuente es obligatoria.");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == nuevaRecomendacion.UsuarioID);
            if (!usuarioExiste)
                return BadRequest("El usuario especificado no existe.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == nuevaRecomendacion.LibroID);
            if (!libroExiste)
                return BadRequest("El libro especificado no existe.");

            var recomendacion = new Recomendacion
            {
                UsuarioID = nuevaRecomendacion.UsuarioID,
                LibroID = nuevaRecomendacion.LibroID,
                Fuente = nuevaRecomendacion.Fuente.Trim()
            };

            _context.Recomendaciones.Add(recomendacion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al guardar la recomendación en la base de datos.");
            }

            nuevaRecomendacion.RecomendacionID = recomendacion.RecomendacionID;

            return CreatedAtAction(nameof(Get), new { id = recomendacion.RecomendacionID }, nuevaRecomendacion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] RecomendacionDTO recomendacionModificada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != recomendacionModificada.RecomendacionID)
                return BadRequest("El ID de la ruta debe coincidir con el ID de la recomendación.");

            var recomendacion = await _context.Recomendaciones.FindAsync(id);
            if (recomendacion == null)
                return NotFound();

            if (recomendacionModificada.UsuarioID <= 0)
                return BadRequest("UsuarioID debe ser un valor positivo.");

            if (recomendacionModificada.LibroID <= 0)
                return BadRequest("LibroID debe ser un valor positivo.");

            if (string.IsNullOrWhiteSpace(recomendacionModificada.Fuente))
                return BadRequest("La fuente es obligatoria.");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == recomendacionModificada.UsuarioID);
            if (!usuarioExiste)
                return BadRequest("El usuario especificado no existe.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == recomendacionModificada.LibroID);
            if (!libroExiste)
                return BadRequest("El libro especificado no existe.");

            recomendacion.UsuarioID = recomendacionModificada.UsuarioID;
            recomendacion.LibroID = recomendacionModificada.LibroID;
            recomendacion.Fuente = recomendacionModificada.Fuente.Trim();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al actualizar la recomendación en la base de datos.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var recomendacion = await _context.Recomendaciones.FindAsync(id);
            if (recomendacion == null)
                return NotFound();

            _context.Recomendaciones.Remove(recomendacion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al eliminar la recomendación en la base de datos.");
            }

            return NoContent();
        }
    }
}
