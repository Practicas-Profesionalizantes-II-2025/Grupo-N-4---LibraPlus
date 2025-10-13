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

        // ✅ GET api/prestamos
        // Devuelve todos los préstamos o los del usuario actual si viene ?usuarioId=#
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? usuarioId)
        {
            var prestamos = usuarioId.HasValue
                ? await _prestamosService.GetPrestamosPorUsuarioAsync(usuarioId.Value)
                : await _prestamosService.GetAllAsync();

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

        // ✅ POST api/prestamos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrestamosDTO dto)
        {
            if (dto == null) return BadRequest("Datos inválidos.");

            var prestamo = await _prestamosService.PrestarLibroAsync(dto.UsuarioID, dto.LibroID, dto.FechaFin);
            if (prestamo == null) return BadRequest("No se pudo crear el préstamo.");

            dto.PrestamoID = prestamo.PrestamoID;
            dto.FechaInicio = prestamo.FechaInicio;
            dto.Devuelto = prestamo.Devuelto;

            return CreatedAtAction(nameof(GetById), new { id = dto.PrestamoID }, dto);
        }

        // ✅ GET api/prestamos/{id}
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

        // ✅ PUT api/prestamos/{id}/devolver
        [HttpPut("{id}/devolver")]
        public async Task<IActionResult> MarcarComoDevuelto(int id)
        {
            var result = await _prestamosService.MarcarComoDevueltoAsync(id);
            if (!result) return NotFound(new { mensaje = "No se encontró el préstamo o ya fue devuelto." });

            return Ok(new { mensaje = "✅ Préstamo marcado como devuelto correctamente." });
        }

        // 🔎 GET api/prestamos/pendientes
        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes()
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
    }
}
