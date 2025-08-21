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
        Task<Compras> ComprarLibroDigitalAsync(int usuarioID, int libroID);
        Task<IEnumerable<Compras>> GetComprasPorUsuarioAsync(int usuarioId);
        Task<Compras?> GetByIdAsync(int compraId);
    }
}
