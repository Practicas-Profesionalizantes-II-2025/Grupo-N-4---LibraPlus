using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Interfaces
{
    public interface ILibros
    {
        Task<Libros> GetByIdAsync(int id);
        Task<IEnumerable<Libros>> GetAllAsync();
        Task<Libros> AddAsync(Libros libro);
        Task<Libros> UpdateAsync(Libros libro);
        Task<bool> DeleteAsync(int id);
    }
}
