using Microsoft.AspNetCore.Mvc;
using LibraPlus.Aplicacion.DTOs;
using LibraPlus.Aplicacion.Interfaces;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/reseñas")] // ruta fija (permite ñ)
    public class ReseñasController : ControllerBase
    {
        private readonly IReseñas _reseñasService;

        public ReseñasController(IReseñas reseñasService)
        {
            _reseñasService = reseñasService;
        }

        // ✅ GET: api/reseñas/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var reseñas = await _reseñasService.GetAllAsync();

                if (reseñas == null || !reseñas.Any())
                    return Ok(new List<object>()); // devolver JSON vacío válido

                var result = reseñas.Select(r => new ReseñasDTO
                {
                    ReseñaID = r.ReseñaID,
                    UsuarioID = r.UsuarioID,
                    UsuarioNombre = r.Usuario != null ? r.Usuario.Nombre : "Desconocido",
                    LibroID = r.LibroID,
                    LibroTitulo = r.Libro != null ? r.Libro.Titulo : "Sin título",
                    Comentario = r.Comentario,
                    Puntuacion = r.Puntuacion
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "❌ Error al obtener las reseñas.", detalle = ex.Message });
            }
        }


        // ✅ GET: api/reseñas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var reseña = await _reseñasService.GetByIdAsync(id);
                if (reseña == null)
                    return NotFound(new { mensaje = "❌ Reseña no encontrada." });

                return Ok(reseña);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "❌ Error al obtener la reseña.", detalle = ex.Message });
            }
        }

        // ✅ POST: api/reseñas
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReseñasDTO dto)
        {
            if (dto == null)
                return BadRequest(new { mensaje = "❌ Datos inválidos." });

            try
            {
                var reseña = await _reseñasService.CrearReseñaAsync(
                    dto.UsuarioID, dto.LibroID, dto.Comentario, dto.Puntuacion
                );

                dto.ReseñaID = reseña.ReseñaID;

                return CreatedAtAction(nameof(GetById), new { id = dto.ReseñaID }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "❌ Error al crear la reseña.", detalle = ex.Message });
            }
        }

        // ✅ DELETE: api/reseñas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _reseñasService.DeleteAsync(id);

                if (!eliminado)
                    return NotFound(new { mensaje = "❌ Reseña no encontrada." });

                // ✅ devolvemos siempre JSON (evita error "Unexpected end of JSON input")
                return Ok(new { mensaje = "✅ Reseña eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "❌ Error al eliminar la reseña.", detalle = ex.Message });
            }
        }
    }
}
