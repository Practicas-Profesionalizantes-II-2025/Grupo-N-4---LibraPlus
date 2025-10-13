using LibraPlus.Aplicacion.DTOs;
using LibraPlus.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly ICompras _comprasService;

        public ComprasController(ICompras comprasService)
        {
            _comprasService = comprasService;
        }

        // ✅ Obtener todas las compras
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var compras = await _comprasService.GetAllAsync();
            var result = compras.Select(c => new ComprasDTO
            {
                CompraID = c.CompraID,
                UsuarioID = c.UsuarioID,
                LibroID = c.LibroID,
                Precio = c.Precio,
                Fecha = c.Fecha,
                EsDigital = c.EsDigital,
                DescargaURL = c.DescargaURL
            });

            return Ok(result);
        }

        // ✅ Obtener compra por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var compra = await _comprasService.GetByIdAsync(id);
            if (compra == null)
                return NotFound(new { mensaje = "Compra no encontrada." });

            var dto = new ComprasDTO
            {
                CompraID = compra.CompraID,
                UsuarioID = compra.UsuarioID,
                LibroID = compra.LibroID,
                Precio = compra.Precio,
                Fecha = compra.Fecha,
                EsDigital = compra.EsDigital,
                DescargaURL = compra.DescargaURL
            };

            return Ok(dto);
        }

        // ✅ Obtener compras por usuario
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetComprasPorUsuario(int usuarioId)
        {
            var compras = await _comprasService.GetComprasPorUsuarioAsync(usuarioId);
            if (compras == null || !compras.Any())
                return NotFound(new { mensaje = "Este usuario no tiene compras." });

            var result = compras.Select(c => new ComprasDTO
            {
                CompraID = c.CompraID,
                UsuarioID = c.UsuarioID,
                LibroID = c.LibroID,
                Precio = c.Precio,
                Fecha = c.Fecha,
                EsDigital = c.EsDigital,
                DescargaURL = c.DescargaURL
            });

            return Ok(result);
        }

        // ✅ Crear nueva compra
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComprasDTO dto)
        {
            if (dto == null || dto.UsuarioID <= 0 || dto.LibroID <= 0)
                return BadRequest(new { mensaje = "Datos inválidos: se requiere UsuarioID y LibroID." });

            try
            {
                // ⚙️ Lógica del servicio que maneja compra digital o física
                var compra = await _comprasService.ComprarLibroAsync(dto.UsuarioID, dto.LibroID);

                if (compra == null)
                    return BadRequest(new { mensaje = "Error al procesar la compra." });

                var result = new ComprasDTO
                {
                    CompraID = compra.CompraID,
                    UsuarioID = compra.UsuarioID,
                    LibroID = compra.LibroID,
                    Precio = compra.Precio,
                    Fecha = compra.Fecha,
                    EsDigital = compra.EsDigital,
                    DescargaURL = compra.DescargaURL
                };

                return CreatedAtAction(nameof(GetById), new { id = result.CompraID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor.", detalle = ex.Message });
            }
        }

        // ✅ Eliminar compra
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "ID inválido." });

            try
            {
                var ok = await _comprasService.EliminarAsync(id);

                if (ok)
                    return Ok(new { mensaje = "✅ Compra eliminada correctamente." });
                else
                    return BadRequest(new { mensaje = "❌ No se pudo eliminar la compra (no encontrada o relacionada)." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar compra.", detalle = ex.Message });
            }
        }
    }
}
