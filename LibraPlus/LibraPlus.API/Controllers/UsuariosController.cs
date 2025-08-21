using LibraPlus___Practica_Profesionalizante_II;
using LibraPlus.Aplicacion;
using Microsoft.AspNetCore.Mvc;
using LibraPlus.Aplicacion.DTOs;

namespace LibraPlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarios _usuariosService;

        public UsuariosController(IUsuarios usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuariosService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuariosService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuariosDTO usuarioDto)
        {
            var usuario = new Usuarios
            {
                Nombre = usuarioDto.Nombre,
                Email = usuarioDto.Email,
                Reputación = usuarioDto.Reputacion
            };

            await _usuariosService.AddAsync(usuario);
            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuariosDTO usuarioDto)
        {
            var usuario = await _usuariosService.GetByIdAsync(id);
            if (usuario == null) return NotFound();

            usuario.Nombre = usuarioDto.Nombre;
            usuario.Email = usuarioDto.Email;
            usuario.Reputación = usuarioDto.Reputacion;

            await _usuariosService.UpdateAsync(usuario);
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _usuariosService.DeleteAsync(id);
            return NoContent();
        }
    }
}
