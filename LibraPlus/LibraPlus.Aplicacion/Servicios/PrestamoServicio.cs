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

        public PrestamoServicio(IPrestamosRepository prestamosRepository)
        {
            _prestamosRepository = prestamosRepository;
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
            var prestamo = new Prestamos
            {
                UsuarioID = usuarioId,
                LibroID = libroId,
                FechaInicio = DateTime.Now,
                FechaFin = fechaFin,
                Devuelto = false
            };

            await _prestamosRepository.AddAsync(prestamo);
            return prestamo;
        }

        // ✅ Marcar préstamo como devuelto
        public async Task<bool> MarcarComoDevueltoAsync(int id)
        {
            var prestamo = await _prestamosRepository.GetByIdAsync(id);
            if (prestamo == null) return false;

            prestamo.Devuelto = true;
            await _prestamosRepository.UpdateAsync(prestamo);
            return true;
        }
    }
}
