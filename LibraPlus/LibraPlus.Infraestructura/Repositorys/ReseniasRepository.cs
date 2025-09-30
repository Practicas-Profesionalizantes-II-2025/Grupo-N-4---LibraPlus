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
    public class ReseniasRepository : IReseniasRepository
    {
        private readonly ProyectDBContext _context;

        public ReseniasRepository(ProyectDBContext context)
        {
            _context = context;
        }

        public async Task<Reseñas> GetByIdAsync(int id)
        {
            return await _context.Set<Reseñas>()
                                 .FirstOrDefaultAsync(r => r.ReseñaID == id);
        }

        public async Task<IEnumerable<Reseñas>> GetAllAsync()
        {
            return await _context.Set<Reseñas>().ToListAsync();
        }

        public async Task AddAsync(Reseñas resenia)
        {
            await _context.Set<Reseñas>().AddAsync(resenia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reseñas resenia)
        {
            _context.Set<Reseñas>().Update(resenia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var resenia = await GetByIdAsync(id);
            if (resenia != null)
            {
                _context.Set<Reseñas>().Remove(resenia);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Reseñas>> GetByLibroIdAsync(int libroId)
        {
            return await _context.Set<Reseñas>()
                                 .Where(r => r.LibroID == libroId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Reseñas>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Set<Reseñas>()
                                 .Where(r => r.UsuarioID == usuarioId)
                                 .ToListAsync();
        }
    }

}
