using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
