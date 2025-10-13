using LibraPlus.Aplicacion.DTOs;
using LibraPlus.Aplicacion.Interfaces;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus.Infraestructura.Repositorys;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.EntityFrameworkCore;

namespace LibraPlus.Aplicacion.Servicios
{
    public class LibrosService : ILibros
    {
        private readonly ILibrosRepository _repo;

        public LibrosService(ILibrosRepository repo)
        {
            _repo = repo;
        }

        // ✅ Obtener todos
        public async Task<IEnumerable<LibrosDTO>> GetAllAsync()
        {
            var libros = await _repo.GetAllAsync();

            return libros.Select(l => new LibrosDTO
            {
                LibroID = l.LibroID,
                Titulo = l.Titulo,
                Autor = l.Autor,
                Genero = l.Genero,
                Tipo = l.Tipo,
                Precio = l.Precio,
                Stock = l.Stock,
                CantidadCompras = l.Compras?.Count ?? 0,
                CantidadReseñas = l.Reseñas?.Count ?? 0
            });
        }


        // ✅ Obtener por ID
        public async Task<LibrosDTO?> GetByIdAsync(int id)
        {
            var libro = await _repo.GetByIdAsync(id);
            if (libro == null) return null;

            return new LibrosDTO
            {
                LibroID = libro.LibroID,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Genero = libro.Genero,
                Tipo = libro.Tipo,
                Precio = libro.Precio,
                Stock = libro.Stock
            };
        }

        // ✅ Crear
        public async Task<LibrosDTO?> AddAsync(LibrosDTO dto)
        {
            var libro = new Libros
            {
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                Genero = dto.Genero,
                Tipo = dto.Tipo,
                Precio = dto.Precio,
                Stock = dto.Stock
            };

            await _repo.AddAsync(libro);
            dto.LibroID = libro.LibroID;
            return dto;
        }

        // ✅ Actualizar
        public async Task<bool> UpdateAsync(LibrosDTO dto)
        {
            var libro = await _repo.GetByIdAsync(dto.LibroID);
            if (libro == null) return false;

            libro.Titulo = dto.Titulo;
            libro.Autor = dto.Autor;
            libro.Genero = dto.Genero;
            libro.Tipo = dto.Tipo;
            libro.Precio = dto.Precio;
            libro.Stock = dto.Stock;

            await _repo.UpdateAsync(libro);
            return true;
        }
        // ✅ Eliminar normal
        public async Task<bool> DeleteAsync(int id)
        {
            var libro = await _repo.GetByIdAsync(id);
            if (libro == null)
                return false;

            try
            {
                await _repo.DeleteAsync(id);
                return true;
            }
            catch (DbUpdateException)
            {
                // ⚠️ Lanza excepción si hay dependencias (para que el controller la maneje)
                throw new InvalidOperationException("Dependencias detectadas. Requiere confirmación.");
            }
        }

        // ✅ Eliminación forzada (borra todo)
        public async Task<bool> ForceDeleteAsync(int id)
        {
            var libro = await _repo.GetByIdAsync(id);
            if (libro == null)
                return false;

            await _repo.ForceDeleteAsync(id);
            return true;
        }

        public async Task<bool> ActualizarStockAsync(int libroId, int nuevoStock)
        {
            var libro = await _repo.GetByIdAsync(libroId);
            if (libro == null) return false;

            libro.Stock = nuevoStock;
            await _repo.UpdateAsync(libro);
            return true;
        }
    }
}
