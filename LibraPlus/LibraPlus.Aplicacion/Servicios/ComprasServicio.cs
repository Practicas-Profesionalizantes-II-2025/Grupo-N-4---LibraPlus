using LibraPlus.Aplicacion.Interfaces;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // ✅ Registrar una nueva compra
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

            var esDigital = libro.Tipo.Equals("digital", StringComparison.OrdinalIgnoreCase);

            // 📦 Validar y descontar stock solo si es físico
            if (!esDigital)
            {
                if (libro.Stock <= 0)
                    throw new Exception($"El libro '{libro.Titulo}' no tiene stock disponible.");

                // Descontar 1 unidad del stock
                var nuevoStock = libro.Stock - 1;
                var actualizado = await _librosService.ActualizarStockAsync(libroID, nuevoStock);

                if (!actualizado)
                    throw new Exception("No se pudo actualizar el stock del libro.");
            }

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

        // ✅ Obtener todas las compras
        public async Task<List<Compras>> GetAllAsync()
        {
            var compras = await _comprasRepository.GetAllAsync();
            return compras?.ToList() ?? new List<Compras>();
        }

        // ✅ Obtener compras por usuario
        public async Task<IEnumerable<Compras>> GetComprasPorUsuarioAsync(int usuarioId)
        {
            var compras = await _comprasRepository.GetComprasByUsuarioIdAsync(usuarioId);
            return compras ?? new List<Compras>();
        }

        // ✅ Obtener compra por ID
        public async Task<Compras?> GetByIdAsync(int compraId)
        {
            return await _comprasRepository.GetByIdAsync(compraId);
        }

        // ✅ Eliminar compra
        public async Task<bool> EliminarAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la compra no es válido.");

            return await _comprasRepository.EliminarAsync(id);
        }
    }
}
