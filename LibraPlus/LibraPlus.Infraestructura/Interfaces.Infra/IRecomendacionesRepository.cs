using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Interfaces.Infra
{
    public interface IRecomendacionesRepository
    {
        Task<Recomendaciones> GetByIdAsync(int id);
        Task<IEnumerable<Recomendaciones>> GetAllAsync();
        Task AddAsync(Recomendaciones recomendacion);
        Task UpdateAsync(Recomendaciones recomendacion);
        Task DeleteAsync(int id);

        Task<IEnumerable<Recomendaciones>> GetByUsuarioIdAsync(int usuarioId);
    }

}
