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
    public class ReseñasServicio : IReseñas
    {
        private readonly IReseniasRepository _reseniasRepository;

        public ReseñasServicio(IReseniasRepository reseniasRepository)
        {
            _reseniasRepository = reseniasRepository;
        }

        public async Task<Reseñas> CrearReseniaAsync(int usuarioId, int libroId, string comentario, int puntuacion)
        {
            var resenia = new Reseñas
            {
                UsuarioID = usuarioId,
                LibroID = libroId,
                Comentario = comentario,
                Puntuación = puntuacion
            };

            await _reseniasRepository.AddAsync(resenia);
            return resenia;
        }

        public async Task<IEnumerable<Reseñas>> GetReseniasPorLibroAsync(int libroId)
        {
            return await _reseniasRepository.GetByLibroIdAsync(libroId);
        }

        public async Task<IEnumerable<Reseñas>> GetReseniasPorUsuarioAsync(int usuarioId)
        {
            return await _reseniasRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<Reseñas> GetByIdAsync(int id)
        {
            return await _reseniasRepository.GetByIdAsync(id);
        }
    }
}
