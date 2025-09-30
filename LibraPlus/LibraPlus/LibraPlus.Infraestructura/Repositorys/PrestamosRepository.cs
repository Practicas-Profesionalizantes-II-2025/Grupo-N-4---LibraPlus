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
    public class PrestamosRepository : IPrestamosRepository
    {
        private readonly ProyectDBContext _context;

        public PrestamosRepository(ProyectDBContext context)
        {
            _context = context;
        }

        public async Task<Prestamos> GetByIdAsync(int id)
        {
            return await _context.Set<Prestamos>()
                                 .FirstOrDefaultAsync(p => p.PrestamoID == id);
        }

        public async Task<IEnumerable<Prestamos>> GetAllAsync()
        {
            return await _context.Set<Prestamos>().ToListAsync();
        }

        public async Task AddAsync(Prestamos prestamo)
        {
            await _context.Set<Prestamos>().AddAsync(prestamo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prestamos prestamo)
        {
            _context.Set<Prestamos>().Update(prestamo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var prestamo = await GetByIdAsync(id);
            if (prestamo != null)
            {
                _context.Set<Prestamos>().Remove(prestamo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Prestamos>> GetPrestamosByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Set<Prestamos>()
                                 .Where(p => p.UsuarioID == usuarioId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Prestamos>> GetPrestamosPendientesAsync()
        {
            return await _context.Set<Prestamos>()
                                 .Where(p => !p.Devuelto)
                                 .ToListAsync();
        }
    }

}
