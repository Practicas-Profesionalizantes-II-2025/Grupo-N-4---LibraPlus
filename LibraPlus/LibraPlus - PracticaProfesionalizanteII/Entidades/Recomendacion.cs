using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entidades
{
    public class Recomendacion
    {
        [Key]
        public int RecomendacionID { get; set; }
        
        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }
        
        [ForeignKey("Libro")]
        public int LibroID { get; set; }
        public string Fuente { get; set; } // e.g. "por género", "por popularidad", etc.

    }

}
