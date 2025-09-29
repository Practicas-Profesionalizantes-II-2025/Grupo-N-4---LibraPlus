using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Interfaces
{
    public interface IPrestamo
    {
        Task<Prestamos> PrestarLibroAsync(int usuarioId, int libroId, DateTime fechaFin);
        Task<IEnumerable<Prestamos>> GetPrestamosPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Prestamos>> GetPrestamosPendientesAsync();
        Task<Prestamos> GetByIdAsync(int id);
        Task<bool> MarcarComoDevueltoAsync(int id);
    }
}
