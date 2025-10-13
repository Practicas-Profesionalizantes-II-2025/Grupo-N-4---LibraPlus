using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Interfaces
{
    public interface IPrestamo
    {
        //  Crear un nuevo préstamo
        Task<Prestamos> PrestarLibroAsync(int usuarioId, int libroId, DateTime fechaFin);

        //  Obtener todos los préstamos
        Task<IEnumerable<Prestamos>> GetAllAsync();

        //  Obtener préstamos de un usuario específico
        Task<IEnumerable<Prestamos>> GetPrestamosPorUsuarioAsync(int usuarioId);

        //  Obtener préstamos pendientes (no devueltos)
        Task<IEnumerable<Prestamos>> GetPrestamosPendientesAsync();

        //  Obtener un préstamo por ID
        Task<Prestamos> GetByIdAsync(int id);

        // Marcar un préstamo como devuelto
        Task<bool> MarcarComoDevueltoAsync(int id);

    }
}
