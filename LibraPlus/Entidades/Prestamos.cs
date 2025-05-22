using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus
{
    public class Prestamos
    {
        public int PrestamoID { get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Devuelto { get; set; }

        public Usuarios Usuario { get; set; }
        public Libros Libro { get; set; }
    }
}
