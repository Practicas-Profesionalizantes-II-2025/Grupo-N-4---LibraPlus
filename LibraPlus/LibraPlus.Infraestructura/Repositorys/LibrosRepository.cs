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
    public class LibrosRepository : ILibrosRepository
    {
        private readonly ProyectDBContext _context;

        public LibrosRepository(ProyectDBContext context)
        {
            _context = context;
        }

        public async Task<Libros> GetByIdAsync(int id)
        {
            return await _context.Set<Libros>()
                                 .Include(l => l.Compras)
                                 .FirstOrDefaultAsync(l => l.LibroID == id);
        }

        public async Task<IEnumerable<Libros>> GetAllAsync()
        {
            return await _context.Set<Libros>()
                                 .Include(l => l.Compras)
                                 .ToListAsync();
        }

        public async Task AddAsync(Libros libro)
        {
            await _context.Set<Libros>().AddAsync(libro);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Libros libro)
        {
            _context.Set<Libros>().Update(libro);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var libro = await GetByIdAsync(id);
            if (libro != null)
            {
                _context.Set<Libros>().Remove(libro);
                await _context.SaveChangesAsync();
            }
        }
    }

}
