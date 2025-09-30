using LibraPlus.Aplicacion.Interfaces;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<Prestamos>> GetPrestamosPorUsuarioAsync(int usuarioId)
        {
            return await _prestamosRepository.GetPrestamosByUsuarioIdAsync(usuarioId);
        }

        public async Task<IEnumerable<Prestamos>> GetPrestamosPendientesAsync()
        {
            return await _prestamosRepository.GetPrestamosPendientesAsync();
        }

        public async Task<Prestamos> GetByIdAsync(int id)
        {
            return await _prestamosRepository.GetByIdAsync(id);
        }

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
