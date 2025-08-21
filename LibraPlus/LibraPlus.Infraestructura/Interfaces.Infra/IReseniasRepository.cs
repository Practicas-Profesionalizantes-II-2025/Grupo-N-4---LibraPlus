using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Interfaces.Infra
{
    public interface IReseniasRepository
    {
        Task<Reseñas> GetByIdAsync(int id);
        Task<IEnumerable<Reseñas>> GetAllAsync();
        Task AddAsync(Reseñas resenia);
        Task UpdateAsync(Reseñas resenia);
        Task DeleteAsync(int id);

        Task<IEnumerable<Reseñas>> GetByLibroIdAsync(int libroId);
        Task<IEnumerable<Reseñas>> GetByUsuarioIdAsync(int usuarioId);
    }

}
