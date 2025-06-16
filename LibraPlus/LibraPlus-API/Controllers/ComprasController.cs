using LibraPlus_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Entidades;

namespace LibraPlus_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly ProyectDBContext _context;

        public ComprasController(ProyectDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComprasDTO>>> Get()
        {
            var compras = await _context.Compras.ToListAsync();

            var comprasDTO = compras.Select(compra => new ComprasDTO
            {
                CompraID = compra.CompraID,
                UsuarioID = compra.UsuarioID,
                LibroID = compra.LibroID,
                Fecha = compra.Fecha,
                Precio = compra.Precio,
                DescargaURL = compra.DescargaURL
            }).ToList();

            return Ok(comprasDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComprasDTO>> Get(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
                return NotFound();

            var compraDTO = new ComprasDTO
            {
                CompraID = compra.CompraID,
                UsuarioID = compra.UsuarioID,
                LibroID = compra.LibroID,
                Fecha = compra.Fecha,
                Precio = compra.Precio,
                DescargaURL = compra.DescargaURL
            };

            return Ok(compraDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ComprasDTO>> Post([FromBody] ComprasDTO nuevaCompra)
        {
            // Validar modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar Precio positivo
            if (nuevaCompra.Precio < 0)
                return BadRequest("El precio debe ser un valor positivo.");

            // Validar Fecha no nula ni futura
            if (nuevaCompra.Fecha == default || nuevaCompra.Fecha > DateTime.UtcNow)
                return BadRequest("La fecha debe ser válida y no futura.");

            // Validar que Usuario exista
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == nuevaCompra.UsuarioID);
            if (!usuarioExiste)
                return BadRequest($"No existe un usuario con ID {nuevaCompra.UsuarioID}.");

            // Validar que Libro exista
            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == nuevaCompra.LibroID);
            if (!libroExiste)
                return BadRequest($"No existe un libro con ID {nuevaCompra.LibroID}.");

            // Validar DescargaURL (opcional, si viene)
            if (!string.IsNullOrWhiteSpace(nuevaCompra.DescargaURL))
            {
                if (!Uri.IsWellFormedUriString(nuevaCompra.DescargaURL, UriKind.Absolute))
                    return BadRequest("La URL de descarga no tiene un formato válido.");
            }

            var compra = new Compras
            {
                UsuarioID = nuevaCompra.UsuarioID,
                LibroID = nuevaCompra.LibroID,
                Fecha = nuevaCompra.Fecha,
                Precio = nuevaCompra.Precio,
                DescargaURL = nuevaCompra.DescargaURL
            };

            _context.Compras.Add(compra);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Loggear ex.Message si tenés logging
                return StatusCode(500, "Error al guardar la compra en la base de datos.");
            }

            nuevaCompra.CompraID = compra.CompraID;

            return CreatedAtAction(nameof(Get), new { id = compra.CompraID }, nuevaCompra);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ComprasDTO compraModificada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != compraModificada.CompraID)
                return BadRequest("El ID de la ruta debe coincidir con el ID de la compra.");

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
                return NotFound();

            // Validaciones similares a POST
            if (compraModificada.Precio < 0)
                return BadRequest("El precio debe ser un valor positivo.");

            if (compraModificada.Fecha == default || compraModificada.Fecha > DateTime.UtcNow)
                return BadRequest("La fecha debe ser válida y no futura.");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == compraModificada.UsuarioID);
            if (!usuarioExiste)
                return BadRequest($"No existe un usuario con ID {compraModificada.UsuarioID}.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == compraModificada.LibroID);
            if (!libroExiste)
                return BadRequest($"No existe un libro con ID {compraModificada.LibroID}.");

            if (!string.IsNullOrWhiteSpace(compraModificada.DescargaURL))
            {
                if (!Uri.IsWellFormedUriString(compraModificada.DescargaURL, UriKind.Absolute))
                    return BadRequest("La URL de descarga no tiene un formato válido.");
            }

            compra.UsuarioID = compraModificada.UsuarioID;
            compra.LibroID = compraModificada.LibroID;
            compra.Fecha = compraModificada.Fecha;
            compra.Precio = compraModificada.Precio;
            compra.DescargaURL = compraModificada.DescargaURL;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al actualizar la compra en la base de datos.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
                return NotFound();

            _context.Compras.Remove(compra);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al eliminar la compra en la base de datos.");
            }

            return NoContent();
        }
    }

}
