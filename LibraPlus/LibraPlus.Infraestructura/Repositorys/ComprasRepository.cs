using LibraPlus.Infraestructura.Data;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Repositorys
{
    public class ComprasRepository : IComprasRepository
    {
        private readonly ProyectDBContext _context;

        public ComprasRepository(ProyectDBContext context)
        {
            _context = context;
        }

        // ✅ Obtener una compra por ID
        public async Task<Compras> GetByIdAsync(int id)
        {
            return await _context.Set<Compras>()
                .Include(c => c.Usuario)
                .Include(c => c.Libro)
                .FirstOrDefaultAsync(c => c.CompraID == id);
        }

        // ✅ Obtener todas las compras
        public async Task<IEnumerable<Compras>> GetAllAsync()
        {
            return await _context.Set<Compras>()
                .Include(c => c.Usuario)
                .Include(c => c.Libro)
                .ToListAsync();
        }

        // ✅ Agregar nueva compra
        public async Task AddAsync(Compras compra)
        {
            await _context.Set<Compras>().AddAsync(compra);
            await _context.SaveChangesAsync();
        }

        // ✅ Actualizar compra existente
        public async Task UpdateAsync(Compras compra)
        {
            _context.Set<Compras>().Update(compra);
            await _context.SaveChangesAsync();
        }

        // ✅ Obtener compras por ID de usuario
        public async Task<IEnumerable<Compras>> GetComprasByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Set<Compras>()
                .Include(c => c.Libro)
                .Where(c => c.UsuarioID == usuarioId)
                .ToListAsync();
        }

        // ✅ Eliminar compra (usado por toda la capa superior)
        public async Task<bool> EliminarAsync(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null) return false;

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
