using LibraPlus.Infraestructura.Data;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        // ✅ Obtener todos los préstamos
        public async Task<IEnumerable<Prestamos>> GetAllAsync()
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .AsNoTracking()
                .ToListAsync();
        }

        // ✅ Obtener préstamo por ID
        public async Task<Prestamos> GetByIdAsync(int id)
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.PrestamoID == id);
        }

        // ✅ Obtener préstamos por usuario
        public async Task<IEnumerable<Prestamos>> GetPrestamosByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Prestamos
                .Include(p => p.Libro)
                .Where(p => p.UsuarioID == usuarioId)
                .OrderByDescending(p => p.FechaInicio)
                .ToListAsync();
        }

        // ✅ Obtener préstamos pendientes (no devueltos)
        public async Task<IEnumerable<Prestamos>> GetPrestamosPendientesAsync()
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .Where(p => !p.Devuelto)
                .OrderBy(p => p.FechaFin)
                .ToListAsync();
        }

        // ✅ Agregar nuevo préstamo
        public async Task AddAsync(Prestamos prestamo)
        {
            await _context.Prestamos.AddAsync(prestamo);
            await _context.SaveChangesAsync();
        }

        // ✅ Actualizar préstamo existente
        public async Task UpdateAsync(Prestamos prestamo)
        {
            _context.Entry(prestamo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // ✅ Eliminar préstamo (si alguna vez lo necesitás)
        public async Task DeleteAsync(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
