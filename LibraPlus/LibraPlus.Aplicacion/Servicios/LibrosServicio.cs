using LibraPlus.Aplicacion.Interfaces;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Servicios
{
    public class LibrosServicio : ILibros
    {
        private readonly ILibrosRepository _librosRepository;

        public LibrosServicio(ILibrosRepository librosRepository)
        {
            _librosRepository = librosRepository;
        }

        public async Task<Libros> GetByIdAsync(int id)
        {
            return await _librosRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Libros>> GetAllAsync()
        {
            return await _librosRepository.GetAllAsync();
        }

        public async Task<Libros> AddAsync(Libros libro)
        {
            await _librosRepository.AddAsync(libro);
            return libro;
        }

        public async Task<Libros> UpdateAsync(Libros libro)
        {
            await _librosRepository.UpdateAsync(libro);
            return libro;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var libro = await _librosRepository.GetByIdAsync(id);
            if (libro == null) return false;

            await _librosRepository.DeleteAsync(id);
            return true;
        }
    }

}
