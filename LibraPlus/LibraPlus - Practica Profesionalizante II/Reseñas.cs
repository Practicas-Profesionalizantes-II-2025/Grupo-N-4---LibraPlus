using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus___Practica_Profesionalizante_II
{
    public class Reseñas
    {
        [Key]
        public int ReseñaID { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }

        [ForeignKey("Libro")]
        public int LibroID { get; set; }

        public string? Comentario { get; set; }
        public int Puntuación { get; set; } // 1–5
    }
}
