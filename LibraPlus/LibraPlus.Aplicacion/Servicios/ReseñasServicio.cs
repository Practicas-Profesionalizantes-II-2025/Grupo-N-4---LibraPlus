using LibraPlus.Aplicacion.Interfaces;
using LibraPlus.Dominio.Entidades;
using LibraPlus.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraPlus.Aplicacion.Services
{
    public class ReseñasService : IReseñas
    {
        private readonly ProyectDBContext _context;

        public ReseñasService(ProyectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reseñas>> GetAllAsync()
        {
            return await _context.Reseñas
                .Include(r => r.Usuario)
                .Include(r => r.Libro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reseñas>> GetReseñasPorLibroAsync(int libroId)
        {
            return await _context.Reseñas
                .Where(r => r.LibroID == libroId)
                .Include(r => r.Usuario)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reseñas>> GetReseñasPorUsuarioAsync(int usuarioId)
        {
            return await _context.Reseñas
                .Where(r => r.UsuarioID == usuarioId)
                .Include(r => r.Libro)
                .ToListAsync();
        }

        public async Task<Reseñas?> GetByIdAsync(int id)
        {
            return await _context.Reseñas
                .Include(r => r.Usuario)
                .Include(r => r.Libro)
                .FirstOrDefaultAsync(r => r.ReseñaID == id);
        }

        public async Task<Reseñas> CrearReseñaAsync(int usuarioId, int libroId, string? comentario, int puntuacion)
        {
            var nueva = new Reseñas
            {
                UsuarioID = usuarioId,
                LibroID = libroId,
                Comentario = comentario,
                Puntuacion = puntuacion
            };

            _context.Reseñas.Add(nueva);
            await _context.SaveChangesAsync();
            return nueva;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña == null) return false;

            _context.Reseñas.Remove(reseña);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
