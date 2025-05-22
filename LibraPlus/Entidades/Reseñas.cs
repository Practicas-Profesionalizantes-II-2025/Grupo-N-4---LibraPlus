using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus
{
    public class Reseñas
    {
        public int ReseñaID { get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public string Comentario { get; set; }
        public int Puntuación { get; set; } // 1–5

        public Usuarios Usuario { get; set; }
        public Libros Libro { get; set; }
    }
}
