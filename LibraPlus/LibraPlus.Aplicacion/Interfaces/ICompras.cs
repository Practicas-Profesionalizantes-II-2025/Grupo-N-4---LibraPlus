using LibraPlus___Practica_Profesionalizante_II;
using LibraPlus.Infraestructura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Interfaces
{
    public interface ICompras
    {
        Task<List<Compras>> GetAllAsync();
        Task<Compras?> GetByIdAsync(int compraId);
        Task<IEnumerable<Compras>> GetComprasPorUsuarioAsync(int usuarioId);
        Task<Compras> ComprarLibroAsync(int usuarioID, int libroID);
        Task<bool> EliminarAsync(int id);

    }
}
