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
    public class RecomendacionesServicio : IRecomendaciones
    {
        private readonly IRecomendacionesRepository _repository;

        public RecomendacionesServicio(IRecomendacionesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Recomendaciones> CrearRecomendacionAsync(int usuarioID, int libroID, string fuente)
        {
            var recomendacion = new Recomendaciones
            {
                UsuarioID = usuarioID,
                LibroID = libroID,
                Fuente = fuente
            };

            await _repository.AddAsync(recomendacion);
            return recomendacion;
        }

        public async Task<Recomendaciones> UpdateAsync(Recomendaciones recomendacion)
        {
            await _repository.UpdateAsync(recomendacion);
            return recomendacion;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Recomendaciones>> GetRecomendacionesPorUsuarioAsync(int usuarioID)
        {
            return await _repository.GetByUsuarioIdAsync(usuarioID);
        }

        public async Task<Recomendaciones> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Recomendaciones>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }

}
