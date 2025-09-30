using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Interfaces
{
    public interface IRecomendaciones
    {
        Task<Recomendaciones> CrearRecomendacionAsync(int usuarioID, int libroID, string fuente);
        Task<Recomendaciones> UpdateAsync(Recomendaciones recomendacion);
        Task DeleteAsync(int id);
        Task<IEnumerable<Recomendaciones>> GetRecomendacionesPorUsuarioAsync(int usuarioID);
        Task<Recomendaciones> GetByIdAsync(int id);
        Task<IEnumerable<Recomendaciones>> GetAllAsync();
    }
}
