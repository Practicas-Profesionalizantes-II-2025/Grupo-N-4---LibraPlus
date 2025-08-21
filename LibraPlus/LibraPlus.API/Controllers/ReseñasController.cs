using Microsoft.AspNetCore.Mvc;
using LibraPlus.Aplicacion.DTOs;
using LibraPlus.Aplicacion.Interfaces;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReseñiasController : ControllerBase
    {
        private readonly IReseñas _reseniasService;

        public ReseñiasController(IReseñas reseniasService)
        {
            _reseniasService = reseniasService;
        }

        // GET api/resenias/libro/5
        [HttpGet("libro/{libroId}")]
        public async Task<IActionResult> GetPorLibro(int libroId)
        {
            var resenias = await _reseniasService.GetReseniasPorLibroAsync(libroId);
            var result = resenias.Select(r => new ReseñasDTO
            {
                ReseñaID = r.ReseñaID,
                UsuarioID = r.UsuarioID,
                LibroID = r.LibroID,
                Comentario = r.Comentario,
                Puntuacion = r.Puntuación
            });

            return Ok(result);
        }

        // GET api/resenias/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetPorUsuario(int usuarioId)
        {
            var resenias = await _reseniasService.GetReseniasPorUsuarioAsync(usuarioId);
            var result = resenias.Select(r => new ReseñasDTO
            {
                ReseñaID = r.ReseñaID,
                UsuarioID = r.UsuarioID,
                LibroID = r.LibroID,
                Comentario = r.Comentario,
                Puntuacion = r.Puntuación
            });

            return Ok(result);
        }

        // POST api/resenias
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReseñasDTO dto)
        {
            if (dto == null) return BadRequest("Datos inválidos.");

            var resenia = await _reseniasService.CrearReseniaAsync(dto.UsuarioID, dto.LibroID, dto.Comentario, dto.Puntuacion);

            dto.ReseñaID = resenia.ReseñaID;
            return CreatedAtAction(nameof(GetById), new { id = dto.ReseñaID }, dto);
        }

        // GET api/resenias/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resenia = await _reseniasService.GetByIdAsync(id);
            if (resenia == null) return NotFound();

            var dto = new ReseñasDTO
            {
                ReseñaID = resenia.ReseñaID,
                UsuarioID = resenia.UsuarioID,
                LibroID = resenia.LibroID,
                Comentario = resenia.Comentario,
                Puntuacion = resenia.Puntuación
            };

            return Ok(dto);
        }
    }

}
