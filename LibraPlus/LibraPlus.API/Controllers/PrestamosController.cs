using LibraPlus.Aplicacion.DTOs;
using LibraPlus.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamo _prestamosService;

        public PrestamosController(IPrestamo prestamosService)
        {
            _prestamosService = prestamosService;
        }

        // POST api/prestamos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrestamosDTO dto)
        {
            if (dto == null) return BadRequest("Datos inválidos.");

            var prestamo = await _prestamosService.PrestarLibroAsync(dto.UsuarioID, dto.LibroID, dto.FechaFin);

            // Devolvemos DTO completo
            dto.PrestamoID = prestamo.PrestamoID;
            dto.FechaInicio = prestamo.FechaInicio;
            dto.Devuelto = prestamo.Devuelto;

            return CreatedAtAction(nameof(GetById), new { id = dto.PrestamoID }, dto);
        }

        // GET api/prestamos/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetPrestamosPorUsuario(int usuarioId)
        {
            var prestamos = await _prestamosService.GetPrestamosPorUsuarioAsync(usuarioId);

            var result = prestamos.Select(p => new PrestamosDTO
            {
                PrestamoID = p.PrestamoID,
                UsuarioID = p.UsuarioID,
                LibroID = p.LibroID,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                Devuelto = p.Devuelto
            });

            return Ok(result);
        }

        // GET api/prestamos/pendientes
        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPrestamosPendientes()
        {
            var prestamos = await _prestamosService.GetPrestamosPendientesAsync();

            var result = prestamos.Select(p => new PrestamosDTO
            {
                PrestamoID = p.PrestamoID,
                UsuarioID = p.UsuarioID,
                LibroID = p.LibroID,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                Devuelto = p.Devuelto
            });

            return Ok(result);
        }

        // GET api/prestamos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prestamo = await _prestamosService.GetByIdAsync(id);
            if (prestamo == null) return NotFound();

            var dto = new PrestamosDTO
            {
                PrestamoID = prestamo.PrestamoID,
                UsuarioID = prestamo.UsuarioID,
                LibroID = prestamo.LibroID,
                FechaInicio = prestamo.FechaInicio,
                FechaFin = prestamo.FechaFin,
                Devuelto = prestamo.Devuelto
            };

            return Ok(dto);
        }

        // PUT api/prestamos/5/devolver
        [HttpPut("{id}/devolver")]
        public async Task<IActionResult> MarcarComoDevuelto(int id)
        {
            var result = await _prestamosService.MarcarComoDevueltoAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }

}
