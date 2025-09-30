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
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly ProyectDBContext _context;

        public UsuariosRepository(ProyectDBContext context)
        {
            _context = context;
        }

        public async Task<Usuarios> GetByIdAsync(int id)
        {
            return await _context.Set<Usuarios>().FindAsync(id);
        }

        public async Task<IEnumerable<Usuarios>> GetAllAsync()
        {
            return await _context.Set<Usuarios>().ToListAsync();
        }

        public async Task AddAsync(Usuarios usuario)
        {
            await _context.Set<Usuarios>().AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuarios usuario)
        {
            _context.Set<Usuarios>().Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario != null)
            {
                _context.Set<Usuarios>().Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
