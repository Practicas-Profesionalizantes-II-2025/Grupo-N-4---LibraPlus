using LibraPlus_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Entidades;

namespace LibraPlus_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReseñasController : ControllerBase
    {
        private readonly ProyectDBContext _context;

        public ReseñasController(ProyectDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReseñasDTO>>> Get()
        {
            var reseñas = await _context.Reseñas.ToListAsync();

            var dtoList = reseñas.Select(r => new ReseñasDTO
            {
                ResenaID = r.ReseñaID,
                UsuarioID = r.UsuarioID,
                LibroID = r.LibroID,
                Comentario = r.Comentario,
                Puntuacion = r.Puntuación
            }).ToList();

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReseñasDTO>> Get(int id)
        {
            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña == null)
                return NotFound();

            var dto = new ReseñasDTO
            {
                ResenaID = reseña.ReseñaID,
                UsuarioID = reseña.UsuarioID,
                LibroID = reseña.LibroID,
                Comentario = reseña.Comentario,
                Puntuacion = reseña.Puntuación
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ReseñasDTO>> Post([FromBody] ReseñasDTO nuevaResena)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (nuevaResena.UsuarioID <= 0)
                return BadRequest("UsuarioID debe ser un valor positivo.");

            if (nuevaResena.LibroID <= 0)
                return BadRequest("LibroID debe ser un valor positivo.");

            if (string.IsNullOrWhiteSpace(nuevaResena.Comentario))
                return BadRequest("El comentario es obligatorio.");

            if (nuevaResena.Puntuacion < 1 || nuevaResena.Puntuacion > 5)
                return BadRequest("La puntuación debe estar entre 1 y 5.");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == nuevaResena.UsuarioID);
            if (!usuarioExiste)
                return BadRequest("El usuario especificado no existe.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == nuevaResena.LibroID);
            if (!libroExiste)
                return BadRequest("El libro especificado no existe.");

            var reseña = new Reseñas
            {
                UsuarioID = nuevaResena.UsuarioID,
                LibroID = nuevaResena.LibroID,
                Comentario = nuevaResena.Comentario.Trim(),
                Puntuación = nuevaResena.Puntuacion
            };

            _context.Reseñas.Add(reseña);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al guardar la reseña en la base de datos.");
            }

            nuevaResena.ResenaID = reseña.ReseñaID;

            return CreatedAtAction(nameof(Get), new { id = reseña.ReseñaID }, nuevaResena);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ReseñasDTO reseñaModificada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != reseñaModificada.ResenaID)
                return BadRequest("El ID de la ruta debe coincidir con el ID de la reseña.");

            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña == null)
                return NotFound();

            if (reseñaModificada.UsuarioID <= 0)
                return BadRequest("UsuarioID debe ser un valor positivo.");

            if (reseñaModificada.LibroID <= 0)
                return BadRequest("LibroID debe ser un valor positivo.");

            if (string.IsNullOrWhiteSpace(reseñaModificada.Comentario))
                return BadRequest("El comentario es obligatorio.");

            if (reseñaModificada.Puntuacion < 1 || reseñaModificada.Puntuacion > 5)
                return BadRequest("La puntuación debe estar entre 1 y 5.");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == reseñaModificada.UsuarioID);
            if (!usuarioExiste)
                return BadRequest("El usuario especificado no existe.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == reseñaModificada.LibroID);
            if (!libroExiste)
                return BadRequest("El libro especificado no existe.");

            reseña.UsuarioID = reseñaModificada.UsuarioID;
            reseña.LibroID = reseñaModificada.LibroID;
            reseña.Comentario = reseñaModificada.Comentario.Trim();
            reseña.Puntuación = reseñaModificada.Puntuacion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al actualizar la reseña en la base de datos.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña == null)
                return NotFound();

            _context.Reseñas.Remove(reseña);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al eliminar la reseña en la base de datos.");
            }

            return NoContent();
        }
    }

}
