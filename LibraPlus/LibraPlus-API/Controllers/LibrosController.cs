using LibraPlus_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
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

        [HttpGet]
        public async Task<ActionResult<List<LibrosDTO>>> Get()
        {
            var libros = await _context.Libros.ToListAsync();

            var librosDTO = libros.Select(libro => new LibrosDTO
            {
                LibroID = libro.LibroID,
                Titulo = libro.Título,
                Autor = libro.Autor,
                Genero = libro.Género,
                Tipo = libro.Tipo,
                Precio = libro.Precio,
                Stock = libro.Stock
            }).ToList();

            return Ok(librosDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibrosDTO>> Get(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound();

            var libroDTO = new LibrosDTO
            {
                LibroID = libro.LibroID,
                Titulo = libro.Título,
                Autor = libro.Autor,
                Genero = libro.Género,
                Tipo = libro.Tipo,
                Precio = libro.Precio,
                Stock = libro.Stock
            };

            return Ok(libroDTO);
        }

        [HttpPost]
        public async Task<ActionResult<LibrosDTO>> Post([FromBody] LibrosDTO nuevoLibro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validaciones manuales adicionales

            if (string.IsNullOrWhiteSpace(nuevoLibro.Titulo))
                return BadRequest("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(nuevoLibro.Autor))
                return BadRequest("El autor es obligatorio.");

            if (string.IsNullOrWhiteSpace(nuevoLibro.Genero))
                return BadRequest("El género es obligatorio.");

            if (string.IsNullOrWhiteSpace(nuevoLibro.Tipo))
                return BadRequest("El tipo es obligatorio.");

            if (nuevoLibro.Precio < 0)
                return BadRequest("El precio debe ser positivo.");

            if (nuevoLibro.Stock < 0)
                return BadRequest("El stock no puede ser negativo.");

            var libro = new Libros
            {
                Título = nuevoLibro.Titulo,
                Autor = nuevoLibro.Autor,
                Género = nuevoLibro.Genero,
                Tipo = nuevoLibro.Tipo,
                Precio = nuevoLibro.Precio,
                Stock = nuevoLibro.Stock
            };

            _context.Libros.Add(libro);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al guardar el libro en la base de datos.");
            }

            nuevoLibro.LibroID = libro.LibroID;

            return CreatedAtAction(nameof(Get), new { id = libro.LibroID }, nuevoLibro);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] LibrosDTO libroModificado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != libroModificado.LibroID)
                return BadRequest("El ID de la ruta debe coincidir con el ID del libro.");

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(libroModificado.Titulo))
                return BadRequest("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(libroModificado.Autor))
                return BadRequest("El autor es obligatorio.");

            if (string.IsNullOrWhiteSpace(libroModificado.Genero))
                return BadRequest("El género es obligatorio.");

            if (string.IsNullOrWhiteSpace(libroModificado.Tipo))
                return BadRequest("El tipo es obligatorio.");

            if (libroModificado.Precio < 0)
                return BadRequest("El precio debe ser positivo.");

            if (libroModificado.Stock < 0)
                return BadRequest("El stock no puede ser negativo.");

            libro.Título = libroModificado.Titulo;
            libro.Autor = libroModificado.Autor;
            libro.Género = libroModificado.Genero;
            libro.Tipo = libroModificado.Tipo;
            libro.Precio = libroModificado.Precio;
            libro.Stock = libroModificado.Stock;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al actualizar el libro en la base de datos.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound();

            _context.Libros.Remove(libro);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al eliminar el libro en la base de datos.");
            }

            return NoContent();
        }
    }

}
