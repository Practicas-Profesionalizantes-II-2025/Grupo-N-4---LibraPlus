using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.DTOs
{
    public class RecomendacionesDTO
    {
        public int RecomendacionID { get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public string Fuente { get; set; }
    }
}
