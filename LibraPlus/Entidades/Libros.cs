﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus
{
    public class Libros
    {
        public int LibroID { get; set; }
        public string Título { get; set; }
        public string Autor { get; set; }
        public string Género { get; set; }
        public string Tipo { get; set; } // "Digital" o "Físico"
        public decimal Precio { get; set; }
        public int Stock { get; set; }

    }
}
