using LibraPlus.Infraestructura.Data;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        // ✅ Obtener por ID
        public async Task<Libros> GetByIdAsync(int id)
        {
            return await _context.Libros
                .Include(l => l.Compras)
                .Include(l => l.Reseñas)
                .FirstOrDefaultAsync(l => l.LibroID == id);
        }

        // ✅ Obtener todos los libros
        public async Task<IEnumerable<Libros>> GetAllAsync()
        {
            return await _context.Libros
                .Include(l => l.Compras)
                .Include(l => l.Reseñas)
                .AsNoTracking()
                .ToListAsync();
        }


        // ✅ Agregar nuevo libro
        public async Task AddAsync(Libros libro)
        {
            await _context.Libros.AddAsync(libro);
            await _context.SaveChangesAsync();
        }

        // ✅ Actualizar libro existente
        public async Task UpdateAsync(Libros libro)
        {
            var existente = await _context.Libros.FindAsync(libro.LibroID);
            if (existente != null)
            {
                _context.Entry(existente).CurrentValues.SetValues(libro);
                await _context.SaveChangesAsync();
            }
        }

        // ✅ Eliminar libro normal
        public async Task DeleteAsync(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Compras)
                .Include(l => l.Reseñas)
                .FirstOrDefaultAsync(l => l.LibroID == id);

            if (libro == null)
                return;

            // ⚠️ Si tiene dependencias, dejamos que la capa superior maneje la excepción
            bool tieneDependencias = (libro.Compras?.Any() == true) || (libro.Reseñas?.Any() == true);
            if (tieneDependencias)
                throw new DbUpdateException("El libro tiene dependencias y no puede eliminarse directamente.");

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
        }

        // ✅ Eliminación forzada (borrado de dependencias)
        public async Task ForceDeleteAsync(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Compras)
                .Include(l => l.Reseñas)
                .FirstOrDefaultAsync(l => l.LibroID == id);

            if (libro != null)
            {
                // 🔥 Borramos dependencias primero
                if (libro.Compras != null && libro.Compras.Any())
                    _context.Compras.RemoveRange(libro.Compras);

                if (libro.Reseñas != null && libro.Reseñas.Any())
                    _context.Reseñas.RemoveRange(libro.Reseñas);

                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }
        }
    }
}
