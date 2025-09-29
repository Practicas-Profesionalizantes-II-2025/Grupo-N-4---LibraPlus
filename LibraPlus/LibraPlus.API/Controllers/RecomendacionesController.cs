using LibraPlus.Aplicacion.DTOs;
using LibraPlus.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecomendacionesController : ControllerBase
    {
        private readonly IRecomendaciones _service;

        public RecomendacionesController(IRecomendaciones service)
        {
            _service = service;
        }

        // GET api/recomendaciones
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var recomendaciones = await _service.GetAllAsync();
            var result = recomendaciones.Select(r => new RecomendacionesDTO
            {
                RecomendacionID = r.RecomendacionID,
                UsuarioID = r.UsuarioID,
                LibroID = r.LibroID,
                Fuente = r.Fuente
            });

            return Ok(result);
        }

        // GET api/recomendaciones/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetPorUsuario(int usuarioId)
        {
            var recomendaciones = await _service.GetRecomendacionesPorUsuarioAsync(usuarioId);
            var result = recomendaciones.Select(r => new RecomendacionesDTO
            {
                RecomendacionID = r.RecomendacionID,
                UsuarioID = r.UsuarioID,
                LibroID = r.LibroID,
                Fuente = r.Fuente
            });

            return Ok(result);
        }

        // GET api/recomendaciones/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var recomendacion = await _service.GetByIdAsync(id);
            if (recomendacion == null) return NotFound();

            var dto = new RecomendacionesDTO
            {
                RecomendacionID = recomendacion.RecomendacionID,
                UsuarioID = recomendacion.UsuarioID,
                LibroID = recomendacion.LibroID,
                Fuente = recomendacion.Fuente
            };

            return Ok(dto);
        }

        // POST api/recomendaciones
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecomendacionesDTO dto)
        {
            if (dto == null) return BadRequest("Datos inválidos.");

            var recomendacion = await _service.CrearRecomendacionAsync(dto.UsuarioID, dto.LibroID, dto.Fuente);

            dto.RecomendacionID = recomendacion.RecomendacionID;
            return CreatedAtAction(nameof(GetById), new { id = dto.RecomendacionID }, dto);
        }

        // PUT api/recomendaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RecomendacionesDTO dto)
        {
            if (id != dto.RecomendacionID) return BadRequest("El ID no coincide.");

            var recomendacion = await _service.GetByIdAsync(id);
            if (recomendacion == null) return NotFound();

            recomendacion.UsuarioID = dto.UsuarioID;
            recomendacion.LibroID = dto.LibroID;
            recomendacion.Fuente = dto.Fuente;

            await _service.UpdateAsync(recomendacion);

            return NoContent();
        }

        // DELETE api/recomendaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recomendacion = await _service.GetByIdAsync(id);
            if (recomendacion == null) return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

}
