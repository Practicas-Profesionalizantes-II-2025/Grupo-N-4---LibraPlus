using LibraPlus.Dominio.Entidades;

namespace LibraPlus.Aplicacion.Interfaces
{
    public interface IReseñas
    {
        Task<Reseñas> CrearReseñaAsync(int usuarioId, int libroId, string comentario, int puntuacion);
        Task<IEnumerable<Reseñas>> GetReseñasPorLibroAsync(int libroId);
        Task<IEnumerable<Reseñas>> GetReseñasPorUsuarioAsync(int usuarioId);
        Task<Reseñas> GetByIdAsync(int id);
        Task<IEnumerable<Reseñas>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
