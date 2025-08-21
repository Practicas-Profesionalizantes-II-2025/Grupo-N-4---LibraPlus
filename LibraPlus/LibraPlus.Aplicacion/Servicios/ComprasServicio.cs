using LibraPlus.Aplicacion.Interfaces;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus.Infraestructura.Repositorys;
using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Servicios
{
    public class ComprasService : ICompras
    {
        private readonly IComprasRepository _comprasRepository;
        private readonly ILibros _librosService;
        private readonly IUsuarios _usuariosService;

        public ComprasService(IComprasRepository comprasRepository, ILibros librosService, IUsuarios usuariosService)
        {
            _comprasRepository = comprasRepository;
            _librosService = librosService;
            _usuariosService = usuariosService;
        }

        public async Task<Compras> ComprarLibroDigitalAsync(int usuarioID, int libroID)
        {
            // Validamos usuario
            var usuario = await _usuariosService.GetByIdAsync(usuarioID);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            // Validamos libro
            var libro = await _librosService.GetByIdAsync(libroID);
            if (libro == null) throw new Exception("Libro no encontrado.");
            if (libro.Tipo != "Digital") throw new Exception("El libro no es digital.");

            // Creamos compra
            var compra = new Compras
            {
                UsuarioID = usuarioID,
                LibroID = libroID,
                Precio = libro.Precio,
                Fecha = DateTime.UtcNow,
                EsDigital = true,
                DescargaURL = $"https://midominio.com/descargas/{Guid.NewGuid()}"
            };

            await _comprasRepository.AddAsync(compra);

            return compra;
        }

        public async Task<IEnumerable<Compras>> GetComprasPorUsuarioAsync(int usuarioId)
        {
            return await _comprasRepository.GetComprasByUsuarioIdAsync(usuarioId);
        }

        public async Task<Compras?> GetByIdAsync(int compraId)
        {
            return await _comprasRepository.GetByIdAsync(compraId);
        }
    }


}
