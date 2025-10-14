using LibraPlus.Aplicacion.Interfaces;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Servicios
{
    public class PrestamoServicio : IPrestamo
    {
        private readonly IPrestamosRepository _prestamosRepository;
        private readonly ILibros _librosService;

        public PrestamoServicio(IPrestamosRepository prestamosRepository, ILibros librosService)
        {
            _prestamosRepository = prestamosRepository;
            _librosService = librosService; // ✅ ahora sí existe
        }

        // ✅ Obtener todos los préstamos
        public async Task<IEnumerable<Prestamos>> GetAllAsync()
        {
            return await _prestamosRepository.GetAllAsync();
        }

        // ✅ Obtener préstamo por ID
        public async Task<Prestamos> GetByIdAsync(int id)
        {
            return await _prestamosRepository.GetByIdAsync(id);
        }

        // ✅ Obtener préstamos por usuario
        public async Task<IEnumerable<Prestamos>> GetPrestamosPorUsuarioAsync(int usuarioId)
        {
            return await _prestamosRepository.GetPrestamosByUsuarioIdAsync(usuarioId);
        }

        // ✅ Obtener préstamos pendientes
        public async Task<IEnumerable<Prestamos>> GetPrestamosPendientesAsync()
        {
            return await _prestamosRepository.GetPrestamosPendientesAsync();
        }

        // ✅ Registrar nuevo préstamo
        public async Task<Prestamos> PrestarLibroAsync(int usuarioId, int libroId, DateTime fechaFin)
        {
            // Primero obtenemos el libro
            var libro = await _librosService.GetByIdAsync(libroId);
            if (libro == null)
                throw new Exception("Libro no encontrado");

            // Solo se presta si es físico y hay stock
            if (libro.Tipo == "Físico" && libro.Stock <= 0)
                throw new Exception("No hay stock disponible para este libro");

            // Crear el préstamo
            var prestamo = new Prestamos
            {
                UsuarioID = usuarioId,
                LibroID = libroId,
                FechaInicio = DateTime.Now,
                FechaFin = fechaFin,
                Devuelto = false
            };

            await _prestamosRepository.AddAsync(prestamo);

            // Descontar 1 del stock si es físico
            if (libro.Tipo == "Físico")
            {
                await _librosService.ActualizarStockAsync(libro.LibroID, libro.Stock - 1);
            }

            return prestamo;
        }

        // ✅ Marcar préstamo como devuelto
        public async Task<bool> MarcarComoDevueltoAsync(int id)
        {
            var prestamo = await _prestamosRepository.GetByIdAsync(id);
            if (prestamo == null || prestamo.Devuelto)
                return false;

            prestamo.Devuelto = true;
            await _prestamosRepository.UpdateAsync(prestamo);

            // Recuperar libro y aumentar stock si es físico
            var libro = await _librosService.GetByIdAsync(prestamo.LibroID);
            if (libro != null && libro.Tipo == "Físico")
            {
                await _librosService.ActualizarStockAsync(libro.LibroID, libro.Stock + 1);
            }

            return true;
        }
    }
}
