using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus___Practica_Profesionalizante_II
{
    public class Prestamos
    {
        [Key]
        public int PrestamoID { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }
        [ForeignKey("Libro")]
        public int LibroID { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Devuelto { get; set; }
    }
}
