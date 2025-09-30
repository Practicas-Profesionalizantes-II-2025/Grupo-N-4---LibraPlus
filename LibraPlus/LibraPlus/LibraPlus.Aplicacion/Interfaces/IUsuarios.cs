using LibraPlus___Practica_Profesionalizante_II;

namespace LibraPlus.Aplicacion
{
    public interface IUsuarios
    {
        Task<Usuarios> GetByIdAsync(int id);
        Task<IEnumerable<Usuarios>> GetAllAsync();
        Task AddAsync(Usuarios usuario);
        Task UpdateAsync(Usuarios usuario);
        Task DeleteAsync(int id);
    }
}
