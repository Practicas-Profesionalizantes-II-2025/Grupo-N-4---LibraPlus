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
            var compras = await _context.Compras
                .Include(c => c.Usuario)
                .Include(c => c.Libro)
                .ToListAsync();

            var comprasDTO = compras.Select(compra => new ComprasDTO
            {
                CompraID = compra.CompraID,
                UsuarioID = compra.UsuarioID,
                NombreUsuario = compra.Usuario.Nombre,
                LibroID = compra.LibroID,
                TituloLibro = compra.Libro.Título,
                Fecha = compra.Fecha,
                Precio = compra.Precio,
                EsDigital = compra.EsDigital,
                DescargaURL = compra.DescargaURL
            }).ToList();

            return Ok(comprasDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComprasDTO>> Get(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.Usuario)
                .Include(c => c.Libro)
                .FirstOrDefaultAsync(c => c.CompraID == id);

            if (compra == null)
                return NotFound();

            var compraDTO = new ComprasDTO
            {
                CompraID = compra.CompraID,
                UsuarioID = compra.UsuarioID,
                NombreUsuario = compra.Usuario.Nombre,
                LibroID = compra.LibroID,
                TituloLibro = compra.Libro.Título,
                Fecha = compra.Fecha,
                Precio = compra.Precio,
                EsDigital = compra.EsDigital,
                DescargaURL = compra.DescargaURL
            };

            return Ok(compraDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ComprasDTO>> Post([FromBody] ComprasDTO nuevaCompra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (nuevaCompra.Precio < 0)
                return BadRequest("El precio debe ser un valor positivo.");

            if (nuevaCompra.Fecha == default || nuevaCompra.Fecha > DateTime.UtcNow)
                return BadRequest("La fecha debe ser válida y no futura.");

            if (nuevaCompra.EsDigital && string.IsNullOrWhiteSpace(nuevaCompra.DescargaURL))
                return BadRequest("La URL de descarga es obligatoria para compras digitales.");

            if (!string.IsNullOrWhiteSpace(nuevaCompra.DescargaURL) &&
                !Uri.IsWellFormedUriString(nuevaCompra.DescargaURL, UriKind.Absolute))
                return BadRequest("La URL de descarga no tiene un formato válido.");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == nuevaCompra.UsuarioID);
            if (!usuarioExiste)
                return BadRequest($"No existe un usuario con ID {nuevaCompra.UsuarioID}.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == nuevaCompra.LibroID);
            if (!libroExiste)
                return BadRequest($"No existe un libro con ID {nuevaCompra.LibroID}.");

            var compra = new Compras
            {
                UsuarioID = nuevaCompra.UsuarioID,
                LibroID = nuevaCompra.LibroID,
                Fecha = nuevaCompra.Fecha,
                Precio = nuevaCompra.Precio,
                EsDigital = nuevaCompra.EsDigital,
                DescargaURL = nuevaCompra.DescargaURL
            };

            _context.Compras.Add(compra);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al guardar la compra en la base de datos.");
            }

            nuevaCompra.CompraID = compra.CompraID;

            // Opcional: podrías completar también NombreUsuario y TituloLibro haciendo otra consulta

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

            if (compraModificada.Precio < 0)
                return BadRequest("El precio debe ser un valor positivo.");

            if (compraModificada.Fecha == default || compraModificada.Fecha > DateTime.UtcNow)
                return BadRequest("La fecha debe ser válida y no futura.");

            if (compraModificada.EsDigital && string.IsNullOrWhiteSpace(compraModificada.DescargaURL))
                return BadRequest("La URL de descarga es obligatoria para compras digitales.");

            if (!string.IsNullOrWhiteSpace(compraModificada.DescargaURL) &&
                !Uri.IsWellFormedUriString(compraModificada.DescargaURL, UriKind.Absolute))
                return BadRequest("La URL de descarga no tiene un formato válido.");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == compraModificada.UsuarioID);
            if (!usuarioExiste)
                return BadRequest($"No existe un usuario con ID {compraModificada.UsuarioID}.");

            var libroExiste = await _context.Libros.AnyAsync(l => l.LibroID == compraModificada.LibroID);
            if (!libroExiste)
                return BadRequest($"No existe un libro con ID {compraModificada.LibroID}.");

            compra.UsuarioID = compraModificada.UsuarioID;
            compra.LibroID = compraModificada.LibroID;
            compra.Fecha = compraModificada.Fecha;
            compra.Precio = compraModificada.Precio;
            compra.EsDigital = compraModificada.EsDigital;
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
