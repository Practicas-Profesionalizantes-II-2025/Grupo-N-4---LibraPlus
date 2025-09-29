using LibraPlus.Aplicacion.DTOs;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.AspNetCore.Mvc;
using LibraPlus.Aplicacion.Interfaces;

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

        // GET api/libros
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var libros = await _librosService.GetAllAsync();
            var result = libros.Select(l => new LibroDto
            {
                LibroID = l.LibroID,
                Titulo = l.Titulo,
                Autor = l.Autor,
                Genero = l.Genero,
                Tipo = l.Tipo,
                Precio = l.Precio,
                Stock = l.Stock
            });

            return Ok(result);
        }

        // GET api/libros/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var libro = await _librosService.GetByIdAsync(id);
            if (libro == null) return NotFound();

            var dto = new LibroDto
            {
                LibroID = libro.LibroID,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Genero = libro.Genero,
                Tipo = libro.Tipo,
                Precio = libro.Precio,
                Stock = libro.Stock
            };

            return Ok(dto);
        }

        // POST api/libros
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LibroDto dto)
        {
            var libro = new Libros
            {
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                Genero = dto.Genero,
                Tipo = dto.Tipo,
                Precio = dto.Precio,
                Stock = dto.Stock
            };

            await _librosService.AddAsync(libro);

            // Devolvemos DTO actualizado con el ID generado
            dto.LibroID = libro.LibroID;
            return CreatedAtAction(nameof(GetById), new { id = dto.LibroID }, dto);
        }

        // PUT api/libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LibroDto dto)
        {
            if (id != dto.LibroID) return BadRequest("El ID no coincide.");

            var libro = await _librosService.GetByIdAsync(id);
            if (libro == null) return NotFound();

            libro.Titulo = dto.Titulo;
            libro.Autor = dto.Autor;
            libro.Genero = dto.Genero;
            libro.Tipo = dto.Tipo;
            libro.Precio = dto.Precio;
            libro.Stock = dto.Stock;

            await _librosService.UpdateAsync(libro);

            return NoContent();
        }

        // DELETE api/libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _librosService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
