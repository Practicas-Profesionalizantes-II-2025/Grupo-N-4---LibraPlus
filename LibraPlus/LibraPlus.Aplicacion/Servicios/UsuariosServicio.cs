using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Servicios
{
    public class UsuariosServivio : IUsuarios
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public UsuariosServivio(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<Usuarios> GetByIdAsync(int id)
        {
            return await _usuariosRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Usuarios>> GetAllAsync()
        {
            return await _usuariosRepository.GetAllAsync();
        }

        public async Task AddAsync(Usuarios usuario)
        {
            await _usuariosRepository.AddAsync(usuario);
        }

        public async Task UpdateAsync(Usuarios usuario)
        {
            await _usuariosRepository.UpdateAsync(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            await _usuariosRepository.DeleteAsync(id);
        }
    }

}
