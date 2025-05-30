﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus
{
    public class Recomendacion
    {
        public int RecomendacionID { get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public string Fuente { get; set; } // e.g. "por género", "por popularidad", etc.

        public Usuarios Usuario { get; set; }
        public Libros Libro { get; set; }
    }
}
