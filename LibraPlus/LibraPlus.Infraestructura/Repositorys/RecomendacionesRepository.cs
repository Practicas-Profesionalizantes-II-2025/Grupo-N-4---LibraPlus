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
    public class RecomendacionesRepository : IRecomendacionesRepository
    {
        private readonly ProyectDBContext _context;

        public RecomendacionesRepository(ProyectDBContext context)
        {
            _context = context;
        }

        public async Task<Recomendaciones> GetByIdAsync(int id)
        {
            return await _context.Set<Recomendaciones>()
                                 .FirstOrDefaultAsync(r => r.RecomendacionID == id);
        }

        public async Task<IEnumerable<Recomendaciones>> GetAllAsync()
        {
            return await _context.Set<Recomendaciones>().ToListAsync();
        }

        public async Task AddAsync(Recomendaciones recomendacion)
        {
            await _context.Set<Recomendaciones>().AddAsync(recomendacion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recomendaciones recomendacion)
        {
            _context.Set<Recomendaciones>().Update(recomendacion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recomendacion = await GetByIdAsync(id);
            if (recomendacion != null)
            {
                _context.Set<Recomendaciones>().Remove(recomendacion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Recomendaciones>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Set<Recomendaciones>()
                                 .Where(r => r.UsuarioID == usuarioId)
                                 .ToListAsync();
        }
    }

}
