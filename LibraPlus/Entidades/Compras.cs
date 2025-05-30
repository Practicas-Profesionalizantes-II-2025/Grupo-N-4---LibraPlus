﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus
{
    public class Compras
    {
        public int CompraID { get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public string DescargaURL { get; set; }

        public Usuarios Usuario { get; set; }
        public Libros Libro { get; set; }
    }
}
