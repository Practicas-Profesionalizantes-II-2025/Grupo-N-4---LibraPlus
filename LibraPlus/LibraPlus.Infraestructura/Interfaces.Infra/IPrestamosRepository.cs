using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Interfaces.Infra
{
    public interface IPrestamosRepository
    {
        Task<Prestamos> GetByIdAsync(int id);
        Task<IEnumerable<Prestamos>> GetAllAsync();
        Task AddAsync(Prestamos prestamo);
        Task UpdateAsync(Prestamos prestamo);
        Task DeleteAsync(int id);

        Task<IEnumerable<Prestamos>> GetPrestamosByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Prestamos>> GetPrestamosPendientesAsync();
    }

}
