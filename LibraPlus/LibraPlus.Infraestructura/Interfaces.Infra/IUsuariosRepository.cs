using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraPlus___Practica_Profesionalizante_II;

namespace LibraPlus.Infraestructura.Interfaces.Infra
{
    public interface IUsuariosRepository
    {
        Task<Usuarios> GetByIdAsync(int id);
        Task<IEnumerable<Usuarios>> GetAllAsync();
        Task AddAsync(Usuarios usuario);
        Task UpdateAsync(Usuarios usuario);
        Task DeleteAsync(int id);
    }
}
