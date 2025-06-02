using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entidades
{
    public class LibrosDTO
    {
        public int LibroID { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

    }

}
