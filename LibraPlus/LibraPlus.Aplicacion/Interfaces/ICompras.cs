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
        // Para registrar una compra
        Task<Compras> ComprarLibroAsync(int usuarioID, int libroID);

        // Obtener todas las compras
        Task<List<Compras>> GetAllAsync();

        // Obtener compras de un usuario específico
        Task<IEnumerable<Compras>> GetComprasPorUsuarioAsync(int usuarioId);

        // Obtener una compra por su ID
        Task<Compras?> GetByIdAsync(int compraId);
    }
}
