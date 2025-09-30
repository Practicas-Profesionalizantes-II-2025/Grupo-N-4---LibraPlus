using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.Interfaces
{
    public interface IReseñas
    {
        Task<Reseñas> CrearReseniaAsync(int usuarioId, int libroId, string comentario, int puntuacion);
        Task<IEnumerable<Reseñas>> GetReseniasPorLibroAsync(int libroId);
        Task<IEnumerable<Reseñas>> GetReseniasPorUsuarioAsync(int usuarioId);
        Task<Reseñas> GetByIdAsync(int id);
    }
}
