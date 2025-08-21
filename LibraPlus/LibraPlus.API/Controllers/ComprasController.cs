using LibraPlus.Aplicacion.DTOs;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.AspNetCore.Mvc;
using LibraPlus.Aplicacion.Interfaces;

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

        // GET api/compras/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetComprasPorUsuario(int usuarioId)
        {
            var compras = await _comprasService.GetComprasPorUsuarioAsync(usuarioId);

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

        // POST api/compras
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComprasDTO dto)
        {
            if (dto == null)
                return BadRequest("Los datos de la compra son inválidos.");

            // Solo usamos UsuarioID y LibroID que vienen del cliente
            var compra = await _comprasService.ComprarLibroDigitalAsync(dto.UsuarioID, dto.LibroID);

            // Creamos el objeto de salida (Completo)
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

        // GET api/compras/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var compra = await _comprasService.GetByIdAsync(id);
            if (compra == null) return NotFound();

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
    }

}
