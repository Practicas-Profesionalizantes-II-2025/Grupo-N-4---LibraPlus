using LibraPlus.Aplicacion.DTOs;

namespace LibraPlus.Aplicacion.Interfaces
{
    public interface ILibros
    {
        Task<IEnumerable<LibrosDTO>> GetAllAsync();
        Task<LibrosDTO?> GetByIdAsync(int id);
        Task<LibrosDTO?> AddAsync(LibrosDTO dto);
        Task<bool> UpdateAsync(LibrosDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ForceDeleteAsync(int id);
        Task<bool> ActualizarStockAsync(int libroId, int nuevoStock);
    }
}
