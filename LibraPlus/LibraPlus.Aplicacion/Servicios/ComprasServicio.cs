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

        // Registrar una nueva compra
        public async Task<Compras> ComprarLibroAsync(int usuarioID, int libroID)
        {
            // Validar usuario
            var usuario = await _usuariosService.GetByIdAsync(usuarioID);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            // Validar libro
            var libro = await _librosService.GetByIdAsync(libroID);
            if (libro == null)
                throw new Exception("Libro no encontrado.");

            var esDigital = libro.Tipo.ToLower() == "digital";

            // Crear compra
            var compra = new Compras
            {
                UsuarioID = usuarioID,
                LibroID = libroID,
                Precio = libro.Precio,
                Fecha = DateTime.UtcNow,
                EsDigital = esDigital,
                DescargaURL = esDigital
                    ? $"https://libraplus.com/download/{usuarioID}/{libroID}-{Guid.NewGuid()}"
                    : null
            };

            await _comprasRepository.AddAsync(compra);
            return compra;
        }

        // Nuevo método para traer TODAS las compras
        public async Task<List<Compras>> GetAllAsync()
        {
            var compras = await _comprasRepository.GetAllAsync();
            return compras.ToList(); // Convertimos de IEnumerable a List si hace falta
        }

        // Obtener compras de un usuario específico
        public async Task<IEnumerable<Compras>> GetComprasPorUsuarioAsync(int usuarioId)
        {
            return await _comprasRepository.GetComprasByUsuarioIdAsync(usuarioId);
        }

        // Obtener compra por ID
        public async Task<Compras?> GetByIdAsync(int compraId)
        {
            return await _comprasRepository.GetByIdAsync(compraId);
        }
    }
}
