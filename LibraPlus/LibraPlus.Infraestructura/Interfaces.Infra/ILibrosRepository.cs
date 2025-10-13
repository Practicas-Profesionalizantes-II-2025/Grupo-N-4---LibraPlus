using LibraPlus___Practica_Profesionalizante_II;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Interfaces.Infra
{
    public interface ILibrosRepository
    {
        Task<Libros> GetByIdAsync(int id);
        Task<IEnumerable<Libros>> GetAllAsync();
        Task AddAsync(Libros libro);
        Task UpdateAsync(Libros libro);
        Task DeleteAsync(int id);

        // 🆕 Soporte para eliminación forzada
        Task ForceDeleteAsync(int id);
    }
}
