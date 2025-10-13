using LibraPlus.Aplicacion.DTOs;
using LibraPlus.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibros _librosService;

        public LibrosController(ILibros librosService)
        {
            _librosService = librosService;
        }

        // ✅ GET: api/libros
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var libros = await _librosService.GetAllAsync();
            return Ok(libros);
        }

        // ✅ GET: api/libros/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var libro = await _librosService.GetByIdAsync(id);
            if (libro == null)
                return NotFound($"No se encontró el libro con ID {id}");
            return Ok(libro);
        }

        // ✅ POST: api/libros
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LibrosDTO libroDto)
        {
            if (libroDto == null)
                return BadRequest("Datos inválidos.");

            var creado = await _librosService.AddAsync(libroDto);
            if (creado == null)
                return BadRequest("Error al crear el libro.");

            return CreatedAtAction(nameof(GetById), new { id = creado.LibroID }, creado);
        }

        // ✅ PUT: api/libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LibrosDTO libroDto)
        {
            if (libroDto == null || id != libroDto.LibroID)
                return BadRequest("Los datos son incorrectos.");

            var actualizado = await _librosService.UpdateAsync(libroDto);
            if (!actualizado)
                return NotFound($"No se encontró el libro con ID {id} para actualizar.");

            return Ok();
        }

        // 🧨 DELETE normal (detecta dependencias)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _librosService.DeleteAsync(id);
                if (!eliminado)
                    return NotFound($"No se encontró el libro con ID {id}.");

                return Ok("✅ Libro eliminado correctamente");
            }
            catch (InvalidOperationException ex)
            {
                // ⚠️ Dependencias encontradas → pedimos confirmación
                return Conflict(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Error: {ex.Message}");
            }
        }

        // 🔥 DELETE forzado
        [HttpDelete("force/{id}")]
        public async Task<IActionResult> ForceDelete(int id)
        {
            var eliminado = await _librosService.ForceDeleteAsync(id);
            if (!eliminado)
                return NotFound($"No se encontró el libro con ID {id} para eliminar forzadamente.");

            return Ok("✅ Libro y dependencias eliminados correctamente.");
        }
    }
}
