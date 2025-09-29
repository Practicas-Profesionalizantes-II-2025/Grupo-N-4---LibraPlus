using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.DTOs
{
    public class ReseñasDTO
    {
        public int ReseñaID { get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public string? Comentario { get; set; }
        public int Puntuacion { get; set; }
    }
}
