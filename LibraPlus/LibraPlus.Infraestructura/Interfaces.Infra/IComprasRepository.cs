using LibraPlus___Practica_Profesionalizante_II;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Interfaces.Infra
{
    public interface IComprasRepository
    {
        Task<Compras> GetByIdAsync(int id);
        Task<IEnumerable<Compras>> GetAllAsync();
        Task AddAsync(Compras compra);
        Task UpdateAsync(Compras compra);
        Task<IEnumerable<Compras>> GetComprasByUsuarioIdAsync(int usuarioId);
        Task<bool> EliminarAsync(int id);
    }
}
