using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entidades
{
    public class Libros
    {
        [Key]
        public int LibroID { get; set; }
        public string Título { get; set; }
        public string Autor { get; set; }
        public string Género { get; set; }
        public string Tipo { get; set; } // "Digital" o "Físico"
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public virtual ICollection<Compras> Compras { get; set; }

    }

}
