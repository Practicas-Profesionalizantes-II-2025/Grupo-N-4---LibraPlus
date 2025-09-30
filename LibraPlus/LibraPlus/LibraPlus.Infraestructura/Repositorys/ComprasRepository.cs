using LibraPlus.Infraestructura.Data;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<Compras> GetByIdAsync(int id)
        {
            return await _context.Set<Compras>()
                                 .Include(c => c.Usuario)
                                 .Include(c => c.Libro)
                                 .FirstOrDefaultAsync(c => c.CompraID == id);
        }

        public async Task<IEnumerable<Compras>> GetAllAsync()
        {
            return await _context.Set<Compras>()
                                 .Include(c => c.Usuario)
                                 .Include(c => c.Libro)
                                 .ToListAsync();
        }

        public async Task AddAsync(Compras compra)
        {
            await _context.Set<Compras>().AddAsync(compra);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Compras compra)
        {
            _context.Set<Compras>().Update(compra);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var compra = await GetByIdAsync(id);
            if (compra != null)
            {
                _context.Set<Compras>().Remove(compra);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Compras>> GetComprasByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Set<Compras>()
                                 .Include(c => c.Libro)
                                 .Where(c => c.UsuarioID == usuarioId)
                                 .ToListAsync();
        }
    }

}
